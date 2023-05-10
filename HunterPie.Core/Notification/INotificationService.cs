using System;

namespace HunterPie.Core.Notification;
public interface INotificationService
{
    public void Show(string title, string message, TimeSpan visibility);
    public void Info(string title, string message, TimeSpan visibility);
    public void Success(string title, string message, TimeSpan visibility);
    public void Error(string title, string message, TimeSpan visibility);
}
