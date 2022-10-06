using HunterPie.GUI.Parts.Notifications.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Notifications.Views;

/// <summary>
/// Interaction logic for NotificationsPanelView.xaml
/// </summary>
public partial class NotificationsPanelView : UserControl
{
    private NotificationsPanelViewModel ViewModel => (NotificationsPanelViewModel)DataContext;

    public NotificationsPanelView()
    {
        DataContext = new NotificationsPanelViewModel();
        InitializeComponent();
    }
    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        await ViewModel.FetchNotifications();
    }
}
