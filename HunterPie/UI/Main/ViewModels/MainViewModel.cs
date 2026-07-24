using HunterPie.Features.Notification.Service;
using HunterPie.UI.Architecture;
using HunterPie.UI.Controls.Notification.ViewModels;
using HunterPie.UI.Header.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Main.ViewModels;

internal class MainViewModel(
    HeaderViewModel headerViewModel,
    InAppNotificationService notificationService
) : ViewModel
{
    public HeaderViewModel HeaderViewModel { get; } = headerViewModel;

    public ViewModel? ContentViewModel { get; set => SetValue(ref field, value); }

    public ObservableCollection<ToastViewModel> Notifications => notificationService.Notifications;
}