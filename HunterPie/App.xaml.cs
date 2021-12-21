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

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProcessManager _process;
        private Context _context;

        public App()
        {
#if DEBUG
            InitializeLogger();
#endif
            InitializeBuiltinLogger();
            InitializeExceptionsCatcher();
            InitializeDialogManager();
            InitializeProcessScanner();
            InitializeUITracer();
            ClientConfig.Initialize();
            ConfigManager.Initialize();
        }

        private static void InitializeLogger()
        {
            ILogger logger = new NativeLogger();
            Log.Add(logger);

            Log.Info("Hello world! HunterPie stdout has been initialized!");
        }

        private static void InitializeBuiltinLogger()
        {
            ILogger logger = new HunterPieLogger();
            Log.Add(logger);

            Log.Info("Initialized HunterPie logger");
        }

        private void InitializeProcessScanner()
        {
            ProcessManager.OnProcessFound += OnProcessFound;
            ProcessManager.OnProcessClosed += OnProcessClosed;

            ProcessManager.Start();
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
            
        }

        private void InitializeExceptionsCatcher()
        {
            AppDomain.CurrentDomain.FirstChanceException += (_, args) =>
            {
                Log.Error(args.Exception);
            };
        }

        private void InitializeUITracer()
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new LogTracer());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
        }

        private void InitializeDialogManager()
        {
            INativeDialogFactory factory = new UIDialogFactory();
            _ = new DialogManager(factory);
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
