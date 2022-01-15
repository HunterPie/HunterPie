using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Windows;
using HunterPie.Internal.Logger;
using System;
using System.Windows;
using System.Windows.Threading;
using HunterPie.UI.Logger;
using System.Diagnostics;
using HunterPie.Domain.Logger;
using HunterPie.Core.Domain.Dialog;
using HunterPie.UI.Dialog;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Client;
using HunterPie.Core.System;
using HunterPie.Core.Events;
using HunterPie.Internal;
using System.Windows.Navigation;
using System.Windows.Media;
using HunterPie.Core.Client.Configuration.Enums;
using System.Windows.Interop;
using HunterPie.UI.Overlay.Widgets.Monster;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProcessManager _process;
        private Context _context;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializerManager.Initialize();
            SetRenderingMode();

            InitializeProcessScanners();
        }

        private void InitializeProcessScanners()
        {
            ProcessManager.Start();
            ProcessManager.OnProcessFound += OnProcessFound;
            ProcessManager.OnProcessClosed += OnProcessClosed;
        }

        private static void SetRenderingMode()
        {
            RenderOptions.ProcessRenderMode = ClientConfig.Config.Client.Rendering == RenderingStrategy.Hardware
                ? RenderMode.Default
                : RenderMode.SoftwareOnly;
        }

        private void OnProcessClosed(object sender, ProcessManagerEventArgs e)
        {
            if (_process is null)
                return;

            _process = null;
            _context = null;
        }

        private void OnProcessFound(object sender, ProcessManagerEventArgs e)
        {
            if (_process is not null)
            {
                Log.Info("HunterPie is already hooked to another process.");
                return;
            }

            _process = e.Process;
            Context context = GameManager.GetGameContext(e.ProcessName, _process);
            
            Log.Debug("Initialized game context");
            
            _context = context;
            
            Dispatcher.BeginInvoke(() =>
            {
                _ = new MonsterWidgetContextHandler(context);
            });
            
        }

        private void OnUIException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception);
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (e.ApplicationExitCode == 0)
            {
                ConfigManager.SaveAll();
            }
            base.OnExit(e);
        }
    }
}
