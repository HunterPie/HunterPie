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

    public static void Show(string message, TimeSpan visibility) =>
        _instance?._service.Show(string.Empty, message, visibility);

    public static void Info(string message, TimeSpan visibility) =>
        _instance?._service.Info(string.Empty, message, visibility);

    public static void Error(string message, TimeSpan visibility) =>
        _instance?._service.Error(string.Empty, message, visibility);

    public static void Success(string message, TimeSpan visibility) =>
        _instance?._service.Success(string.Empty, message, visibility);
}
