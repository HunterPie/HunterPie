using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Windows;
using HunterPie.Internal.Logger;
using System;
using System.Windows;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProcessManager process;
        private Context context;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            InitializeLogger();
            InitializeExceptionsCatcher();
            InitializeProcessScanner();
        }

        private static void InitializeLogger()
        {
            ILogger logger = new NativeLogger();
            Log.NewInstance(logger);

            Log.Info("Hello world! HunterPie stdout has been initialized!");
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

        private void OnGameClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnGameStart(object sender, EventArgs e)
        {
            GameManager game = new GameManager(process);

            context = new Context(
                    process,
                    game
                );
        }
        
    }
}
