using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Core.System;
using HunterPie.Features;
using HunterPie.Features.Overlay;
using HunterPie.Integrations.Discord;
using HunterPie.Internal;
using HunterPie.UI.Overlay;
using HunterPie.Update;
using HunterPie.Update.Presentation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace HunterPie;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IProcessManager _process;
    private RichPresence _richPresence;
    private Context _context;

    public static MainWindow UI { get; private set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        CheckForRunningInstances();

        base.OnStartup(e);

        InitializerManager.Initialize();
        SetRenderingMode();

        await SelfUpdate();

        ShutdownMode = ShutdownMode.OnMainWindowClose;

        UI = Dispatcher.Invoke(() => new MainWindow());

        UI.InitializeComponent();

        if (!ClientConfig.Config.Client.EnableSeamlessStartup)
            UI.Show();

        _ = WidgetManager.Instance;

        InitializeProcessScanners();
        SetUIThreadPriority();
    }

    private void CheckForRunningInstances()
    {
        Process[] processes = Process.GetProcessesByName("HunterPie")
            .Where(p => p.Id != Environment.ProcessId
                    && p.MainModule.FileName == Process.GetCurrentProcess().MainModule.FileName)
            .ToArray();

        foreach (Process process in processes)
            process.Kill();
    }

    private void SetUIThreadPriority() => Dispatcher.Thread.Priority = ThreadPriority.Highest;

    private async Task SelfUpdate()
    {
        if (!ClientConfig.Config.Client.EnableAutoUpdate)
            return;

        UpdateViewModel vm = new();
        UpdateView view = new() { DataContext = vm };

        if (!ClientConfig.Config.Client.EnableSeamlessStartup)
            view.Show();

        bool result = await UpdateUseCase.Exec(vm);

        view.Close();

        if (result)
            Restart();

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

    private void OnProcessClosed(object sender, ProcessManagerEventArgs e)
    {
        if (_process is null)
            return;

        UnhookEvents();
        _richPresence?.Dispose();

        ScanManager.Stop();
        _context.Dispose();

        _process = null;
        _context = null;

        Dispatcher.InvokeAsync(WidgetInitializers.Unload);
        WidgetManager.Dispose();
        Log.Info("{0} has been closed", e.ProcessName);
        if (e.Process.HasExitedNormally == false
            && e.Process.Game == GameProcess.MonsterHunterWorld
            && ClientConfig.Config.Client.EnableNativeModule)
        {
            Log.Info(
                "{0} has exited abnormally. If you have not installed Stracker's Loader and CRC bypass mod, turning off \"Enable native module\" in Client Settings may help.",
                e.ProcessName
            );
        }

        if (ClientConfig.Config.Client.ShouldShutdownOnGameExit)
            Dispatcher.Invoke(Shutdown);
    }

    private async void OnProcessFound(object sender, ProcessManagerEventArgs e)
    {
        // Reference: https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming
        // Also, note that this function does not assume it can be called concurrently for now. (esp. the _process variable)
        if (_process is not null)
        {
            Log.Info("HunterPie is already hooked to another process.");
            return;
        }

        try
        {
            _process = e.Process;
            _ = GameManager.InitializeGameData(e.ProcessName);
            Context context = GameManager.GetGameContext(e.ProcessName, _process);

            Log.Debug("Initialized game context");
            _context = context;

            HookEvents();
            _richPresence = DiscordPresenceController.GetPresenceBy(context);

            WidgetManager.Hook(context);

            await ContextInitializers.InitializeAsync(context).ConfigureAwait(false);

            await Dispatcher.InvokeAsync(() => WidgetInitializers.Initialize(context));

            ScanManager.Start();
        }
        catch (Exception ex)
        {
            Log.Error("HunterPie fails to initialize on the {0} process. {1}", e.ProcessName, ex);
        }
    }

    private void OnUIException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log.Error(e.Exception.ToString());
        e.Handled = true;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (e.ApplicationExitCode == 0)
            ConfigManager.SaveAll();

        InitializerManager.Unload();

        base.OnExit(e);
    }

    private void HookEvents()
    {
        _context.Game.Player.OnLogin += OnPlayerLogin;
        _context.Game.Player.OnStageUpdate += OnStageUpdate;
    }

    private void UnhookEvents()
    {
        _context.Game.Player.OnLogin -= OnPlayerLogin;
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;
    }

    private void OnPlayerLogin(object sender, EventArgs e) => Log.Info($"Logged in as {_context.Game.Player.Name}");
    private void OnStageUpdate(object sender, EventArgs e) => Log.Debug(string.Format("StageId: {0} | InHuntingZone: {1}", _context.Game.Player.StageId, _context.Game.Player.InHuntingZone));

    public static void Restart()
    {
        _ = Process.Start(typeof(MainWindow).Assembly.Location.Replace(".dll", ".exe"));
        Current.Shutdown();
    }
}
