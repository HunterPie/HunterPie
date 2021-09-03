using HunterPie.Core.Domain.Process;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Windows;
using HunterPie.Internal.Logger;
using System.Windows;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProcessManager process;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            InitializeLogger();
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
        }
        
    }
}
