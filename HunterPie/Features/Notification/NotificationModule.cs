using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Notification.Service;

namespace HunterPie.Features.Notification;

internal class NotificationModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithSingle<InAppNotificationService>();
    }
}