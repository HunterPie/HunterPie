using HunterPie.Core.API;
using HunterPie.Core.API.Schemas;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Notifications.ViewModels
{
    internal class NotificationsPanelViewModel : ViewModel
    {
        public ObservableCollection<NotificationViewModel> Notifications { get; } = new ObservableCollection<NotificationViewModel>();

        public async Task FetchNotifications()
        {
            Notification[] notifications = await PoogieApi.GetNotifications();

            foreach (Notification notification in notifications)
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
}
