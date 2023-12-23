using HunterPie.Core.Client;
using HunterPie.UI.Controls.Notification;
using HunterPie.UI.Controls.Notification.ViewModels;
using System.Windows;
using System.Windows.Input;

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

    private void OnNotificationClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Toast { DataContext: ToastViewModel vm })
            return;

        vm.IsVisible = false;
    }
}
