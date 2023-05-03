using HunterPie.Core.Notification;
using HunterPie.Features.Notification.ViewModels;
using HunterPie.UI.Architecture.Dispatchers;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace HunterPie.Features.Notification;

internal class InAppNotificationService : INotificationService
{
    public static ObservableCollection<AppNotificationViewModel> Notifications { get; } = new();

    public void Info(string title, string message, TimeSpan visibility) =>
        SendNotification(AppNotificationType.Info, title, message, visibility);

    public void Success(string title, string message, TimeSpan visibility) =>
        SendNotification(AppNotificationType.Success, title, message, visibility);

    public void Error(string title, string message, TimeSpan visibility) =>
        SendNotification(AppNotificationType.Error, title, message, visibility);

    private void SendNotification(
        AppNotificationType type,
        string title,
        string message,
        TimeSpan visibility
    ) => Application.Current.Dispatcher.Invoke(
            () =>
            {
                lock (Notifications)
                {
                    var notification = new AppNotificationViewModel { Title = title, Message = message, Type = type };

                    _ = new DispatcherJob(() => HandleVisibilityTimeout(notification), visibility);

                    Notifications.Add(notification);
                }
            });

    private static void HandleVisibilityTimeout(AppNotificationViewModel notification)
    {
        lock (Notifications)
            Notifications.Remove(notification);
    }
}