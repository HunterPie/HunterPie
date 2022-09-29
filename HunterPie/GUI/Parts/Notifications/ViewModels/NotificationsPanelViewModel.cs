using HunterPie.Core.API;
using HunterPie.Core.API.Entities;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Notifications.ViewModels;

internal class NotificationsPanelViewModel : ViewModel
{
    public ObservableCollection<NotificationViewModel> Notifications { get; } = new ObservableCollection<NotificationViewModel>();

    public async Task FetchNotifications()
    {
        PoogieApiResult<Notification[]> notifications = await PoogieApi.GetNotifications();

        if (notifications is null || !notifications.Success)
            return;

        foreach (Notification notification in notifications.Response)
        {
            Notifications.Add(new NotificationViewModel
            {
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.NotificationType,
                Date = notification.CreatedAt
            });
        }
    }
}
