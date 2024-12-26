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
using HunterPie.DI;
using HunterPie.Features;
using HunterPie.Features.Backups;
using HunterPie.Features.Debug;
using HunterPie.Features.Overlay;
using HunterPie.Integrations;
using HunterPie.Integrations.Discord;
using HunterPie.Internal;
using HunterPie.Internal.Exceptions;
using HunterPie.Internal.Tray;
using HunterPie.UI.Main.Navigators;
using HunterPie.UI.Main.Views;
using HunterPie.UI.Overlay;
using HunterPie.Usecases;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
    private IProcessManager? _process;
    private RichPresence? _richPresence;
    private Context? _context;

    internal static MainNavigator? MainController { get; private set; }
    public static MainView? Ui => MainController?.View;

    protected override async void OnStartup(StartupEventArgs e)
    {
        CheckForRunningInstances();

        base.OnStartup(e);

        await InitializerManager.InitializeCore();
        DependencyProvider.LoadModules();
        await InitializerManager.InitializeAsync();

        SetRenderingMode();

        ShutdownMode = ShutdownMode.OnMainWindowClose;

        CheckIfHunterPiePathIsSafe();
        SetupFrameRate();
        InitializeMainView();
        SetupTrayIcon();

        InitializerManager.InitializeGUI();
        DebugWidgets.MockIfNeeded();

        InitializeProcessScanners();
        SetUiThreadPriority();
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

    private async void InitializeMainView()
    {
        Log.Info("Initializing HunterPie client UI");
        MainController = DependencyContainer.Get<MainNavigator>();

        MainApplication application = DependencyContainer.Get<MainApplication>();
        await application.Start();

        if (ClientConfig.Config.Client.EnableSeamlessStartup)
            return;

        MainController.View.Show();
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

        MainApplication mainApplication = DependencyContainer.Get<MainApplication>();

        await mainApplication.Restart();

        Current.Shutdown();
    }

    private void Close()
    {
        InitializerManager.Unload();
        Shutdown();
    }
}