using HunterPie.Core.Client;
using System.Windows;

namespace HunterPie.UI.Main.Views;
/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private void OnDragStart(object sender, RoutedEventArgs e) => DragMove();

    private void OnCloseClick(object sender, RoutedEventArgs e) => Close();

    private void OnMinimizeClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;

        if (ClientConfig.Config.Client.MinimizeToSystemTray)
            Hide();
    }
}
