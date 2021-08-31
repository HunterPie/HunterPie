using HunterPie.Core.Logger;
using HunterPie.UI.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeLogger();

            InitializeComponent();
        }

        private void InitializeLogger()
        {
            ILogger logger = new NativeLogger();
            Log.NewInstance(logger);
        }

        private void OnConsoleLoaded(object sender, RoutedEventArgs e)
        {
            ItemsControl console = (ItemsControl)sender;
            console.ItemsSource = NativeLogger.viewModel;
        }
    }
}
