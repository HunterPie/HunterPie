using HunterPie.UI.Architecture;

namespace HunterPie.Features.Notification.ViewModels;

internal class AppNotificationViewModel : ViewModel
{
    private string _title = string.Empty;
    private string _message = string.Empty;
    private AppNotificationType _type;
    private bool _isVisible = true;

    public string Title { get => _title; set => SetValue(ref _title, value); }
    public string Message { get => _message; set => SetValue(ref _message, value); }
    public AppNotificationType Type { get => _type; set => SetValue(ref _type, value); }
    public bool IsVisible { get => _isVisible; set => SetValue(ref _isVisible, value); }
}
