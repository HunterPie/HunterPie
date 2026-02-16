using HunterPie.Integrations.Poogie.Notification.Models;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.Features.Notifications.ViewModels;

internal class NotificationViewModel : ViewModel
{
    public string Title { get; set => SetValue(ref field, value); }
    public string Message { get; set => SetValue(ref field, value); }
    public NotificationType Type { get; set => SetValue(ref field, value); }
    public DateTime Date { get; set => SetValue(ref field, value); }
}