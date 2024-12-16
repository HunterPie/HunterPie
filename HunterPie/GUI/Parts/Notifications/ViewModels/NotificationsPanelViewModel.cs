using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Notification;
using HunterPie.Integrations.Poogie.Notification.Models;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Notifications.ViewModels;

internal class NotificationsPanelViewModel : ViewModel
{
    private readonly PoogieNotificationConnector _notificationConnector = new();

    public ObservableCollection<NotificationViewModel> Notifications { get; } = new ObservableCollection<NotificationViewModel>();

    public async Task FetchNotifications()
    {
        PoogieResult<NotificationResponse[]> notifications = await _notificationConnector.FindAll();

        if (notifications.Response is null)
            return;

        foreach (NotificationResponse notification in notifications.Response)
            Notifications.Add(new NotificationViewModel
            {
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.NotificationType,
                Date = notification.CreatedAt
            });
    }
}