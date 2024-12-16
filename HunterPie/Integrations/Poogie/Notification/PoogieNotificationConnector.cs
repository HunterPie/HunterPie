using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Notification.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Notification;
internal class PoogieNotificationConnector
{
    private readonly PoogieConnector _connector = new();

    private const string NOTIFICATION_ENDPOINT = "/v1/notifications";

    public async Task<PoogieResult<NotificationResponse[]>> FindAll() =>
        await _connector.Get<NotificationResponse[]>(NOTIFICATION_ENDPOINT);
}