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

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProcessManager process;
        private Context context;

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
            process = new WindowsProcessManager();
            process.Initialize();

            process.OnGameStart += OnGameStart;
            process.OnGameClosed += OnGameClosed;
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

        private void OnGameClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void OnGameStart(object sender, EventArgs e)
        {
            GameManager game = new GameManager(process);
            Strings strings = new Strings(process);
            await strings.InitializeGMDs();

            SongSkill songSkills = new SongSkill(process);

            context = new Context(
                    process,
                    strings,
                    songSkills,
                    game
                );

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
