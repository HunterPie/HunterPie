using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Core.System;
using HunterPie.Domain.Sidebar;
using HunterPie.Features;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.Controller;
using HunterPie.Features.Backups;
using HunterPie.Features.Debug;
using HunterPie.Features.Overlay;
using HunterPie.Integrations;
using HunterPie.Integrations.Discord;
using HunterPie.Internal;
using HunterPie.Internal.Exceptions;
using HunterPie.Internal.Tray;
using HunterPie.UI.Header.ViewModels;
using HunterPie.UI.Logging.ViewModels;
using HunterPie.UI.Main;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;
using HunterPie.UI.Navigation;
using HunterPie.UI.Overlay;
using HunterPie.UI.SideBar.ViewModels;
using HunterPie.Update;
using HunterPie.Update.Presentation;
using HunterPie.Usecases;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HunterPie;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly RemoteAccountConfigService RemoteConfigService = new();
    private IProcessManager? _process;
    private RichPresence? _richPresence;
    private Context? _context;
    private readonly AccountController _accountController = new();

    internal static MainController? MainController { get; private set; }
    public static MainView? Ui => MainController?.View;

    protected override async void OnStartup(StartupEventArgs e)
    {
        CheckForRunningInstances();

        base.OnStartup(e);

        await InitializerManager.Initialize();

        UpdateService.CleanupOldFiles();

        SetRenderingMode();

#if RELEASE
        if (await SelfUpdate())
            return;
#endif

        ShutdownMode = ShutdownMode.OnMainWindowClose;

        CheckIfHunterPiePathIsSafe();
        SetupFrameRate();
        InitializeMainView();
        SetupTrayIcon();

        InitializerManager.InitializeGUI();
        DebugWidgets.MockIfNeeded();

        await AccountManager.ValidateSessionToken();

        InitializeProcessScanners();
        SetUiThreadPriority();

#if RELEASE
        UpdateUseCase.OpenPatchNotesIfJustUpdated();
#endif
    }

    protected override void OnExit(ExitEventArgs e)
    {
        InitializerManager.Unload();
    }

    private void CheckIfHunterPiePathIsSafe()
    {
        bool isSafe = VerifyHunterPiePathUseCase.Invoke();

        if (isSafe)
            return;

        DialogManager.Warn(
            "Unsafe path",
            "It looks like you're executing HunterPie directly from the zip file. Please extract it first before running the client.",
            NativeDialogButtons.Accept
        );

        Shutdown();
    }

    private void SetupFrameRate()
    {
        Timeline.DesiredFrameRateProperty.OverrideMetadata(
            forType: typeof(Timeline),
            typeMetadata: new FrameworkPropertyMetadata { DefaultValue = (int)ClientConfig.Config.Client.RenderFramePerSecond.Current }
        );
    }

    private void InitializeMainView()
    {
        Log.Info("Initializing HunterPie client UI");
        var headerViewModel = new HeaderViewModel(_accountController.GetAccountMenuViewModel())
        {
            IsAdmin = ClientInfo.IsAdmin,
            Version = $"v{ClientInfo.Version}",
        };

        var viewModel = new MainViewModel(headerViewModel);
        MainView view = Dispatcher.Invoke(() => new MainView { DataContext = viewModel });
        MainController = new MainController(view, viewModel);

        // Initialize UI navigation
        var sideBarViewModel = new SideBarViewModel(SideBarProvider.SideBar.Elements);
        var mainBodyViewModel = new MainBodyViewModel(sideBarViewModel);
        var mainBodyNavigator = new MainBodyController(mainBodyViewModel);
        Navigator.SetNavigators(mainBodyNavigator, MainController);
        Navigator.Body.Navigate<ConsoleViewModel>();
        Navigator.App.Navigate(mainBodyViewModel);

        if (ClientConfig.Config.Client.EnableSeamlessStartup)
            return;

        view.Show();
    }

    private void CheckForRunningInstances()
    {
        Process[] processes = Process.GetProcessesByName("HunterPie")
            .Where(p => p.Id != Environment.ProcessId
                    && p.MainModule?.FileName == ClientInfo.ClientFileName)
            .ToArray();

        foreach (Process process in processes)
            process.Kill();
    }

    private void SetUiThreadPriority() => Dispatcher.Thread.Priority = ThreadPriority.Highest;

    private static async Task<bool> SelfUpdate()
    {
        if (!ClientConfig.Config.Client.EnableAutoUpdate)
            return false;

        UpdateViewModel vm = new();
        UpdateView view = new() { DataContext = vm };

        if (!ClientConfig.Config.Client.EnableSeamlessStartup)
            view.Show();

        bool result = await UpdateUseCase.Exec(vm);

        view.Close();

        if (!result)
            return result;

        InitializerManager.Unload();
        Restart();

        return result;
    }

    private void InitializeProcessScanners()
    {
        ProcessManager.Start();
        ProcessManager.OnProcessFound += OnProcessFound;
        ProcessManager.OnProcessClosed += OnProcessClosed;
    }

    private static void SetRenderingMode()
    {
        RenderOptions.ProcessRenderMode = ClientConfig.Config.Client.Render == RenderingStrategy.Hardware
            ? RenderMode.Default
            : RenderMode.SoftwareOnly;
    }

    private void OnTrayShowClick(object? sender, EventArgs e)
    {
        if (Ui is null)
            return;

        Ui.Show();
        Ui.WindowState = WindowState.Normal;
        Ui.Focus();
    }

    private void OnTrayClockClick(object? sender, EventArgs e)
    {
        Ui?.Close();
    }

    private void SetupTrayIcon()
    {
        TrayService.AddDoubleClickHandler(OnTrayShowClick);

        TrayService.AddItem("Show")
            .Click += OnTrayShowClick;

        TrayService.AddItem("Close")
            .Click += OnTrayClockClick;
    }

    private void OnProcessClosed(object? sender, ProcessManagerEventArgs e)
    {
        if (_process is null)
            return;

        UnhookEvents();
        _richPresence?.Dispose();
        _richPresence = null;

        ScanManager.Stop();
        _context?.Dispose();

        _process = null;
        _context = null;

        Dispatcher.Invoke(WidgetInitializers.Unload);
        WidgetManager.Dispose();

        Log.Info("{0} has been closed", e.ProcessName);

        SmartEventsTracker.DisposeEvents();

        ContextInitializers.Dispose();

        if (e.Process.HasExitedNormally == false
            && e.Process.Game == GameProcess.MonsterHunterWorld
            && ClientConfig.Config.Client.EnableNativeModule)
            Log.Info(
                "{0} has exited abnormally. If you have not installed Stracker's Loader and CRC bypass mod, turning off \"Enable native module\" in Client Settings may help.",
                e.ProcessName
            );

        if (ClientConfig.Config.Client.ShouldShutdownOnGameExit)
            Dispatcher.Invoke(Close);
    }

    private async void OnProcessFound(object? sender, ProcessManagerEventArgs e)
    {
        if (_process is not null)
        {
            Log.Info("HunterPie is already hooked to another process.");
            return;
        }

        try
        {
            _process = e.Process;
            Context context = GameIntegrationService.CreateNewGameContext(e.ProcessName, _process);

            Log.Debug("Initialized game context");
            _context = context;

            HookEvents();
            _richPresence = DiscordPresenceController.GetPresenceBy(context);

            await Dispatcher.InvokeAsync(() => WidgetManager.Hook(context));

            await ContextInitializers.InitializeAsync(context).ConfigureAwait(false);

            await Dispatcher.InvokeAsync(() => WidgetInitializers.Initialize(context));

            ScanManager.Start();

            Log.Debug("Active events: {0} with {1} total references", SmartEventsTracker.ActiveEvents(), SmartEventsTracker.CountReferences());
        }
        catch (Exception ex)
        {
            Log.Error("HunterPie fails to initialize on the {0} process. {1}", e.ProcessName, ex);
        }

        _ = await GameSaveBackupService.ExecuteBackup().ConfigureAwait(false);
    }

    private void OnUIException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;

        ExceptionTracker.TrackException(e.Exception);
    }

    private void HookEvents()
    {
        _context!.Game.Player.OnLogin += OnPlayerLogin;
        _context.Game.Player.OnStageUpdate += OnStageUpdate;
    }

    private void UnhookEvents()
    {
        _context!.Game.Player.OnLogin -= OnPlayerLogin;
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;
    }

    private void OnPlayerLogin(object? sender, EventArgs e) => Log.Info($"Logged in as {_context!.Game.Player.Name}");
    private void OnStageUpdate(object? sender, EventArgs e) => Log.Debug("StageId: {0} | InHuntingZone: {1}", _context!.Game.Player.StageId, _context.Game.Player.InHuntingZone);

    public static async void Restart()
    {
        Ui?.Dispatcher.InvokeAsync(() => Ui.Hide());

        await RemoteConfigService.UploadClientConfig();

        Process.Start(typeof(App).Assembly.Location.Replace(".dll", ".exe"));
        Current.Shutdown();
    }

    private void Close()
    {
        InitializerManager.Unload();
        Shutdown();
    }
}
