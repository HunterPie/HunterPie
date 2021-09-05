using HunterPie.UI.Logger;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnConsoleLoaded(object sender, RoutedEventArgs e)
        {
            ItemsControl console = (ItemsControl)sender;
            console.ItemsSource = NativeLogger.viewModel;
        }
    }
}
