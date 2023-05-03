using System;

namespace HunterPie.Core.Notification;

#nullable enable
public class NotificationService
{
    private readonly INotificationService _service;
    private static NotificationService? _instance;

    internal NotificationService(INotificationService service)
    {
        _service = service;
        _instance = this;
    }

    public static void Info(string title, string message, TimeSpan visibility) =>
        _instance?._service.Info(title, message, visibility);

    public static void Error(string title, string message, TimeSpan visibility) =>
        _instance?._service.Error(title, message, visibility);

    public static void Success(string title, string message, TimeSpan visibility) =>
        _instance?._service.Success(title, message, visibility);
}
