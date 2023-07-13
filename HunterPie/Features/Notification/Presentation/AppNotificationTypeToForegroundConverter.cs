using HunterPie.Features.Notification.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.Features.Notification.Presentation;
internal class AppNotificationTypeToForegroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AppNotificationType type)
            return type switch
            {
                AppNotificationType.Default => AppNotificationUiConfig.Default.Foreground,
                AppNotificationType.Info => AppNotificationUiConfig.Info.Foreground,
                AppNotificationType.Success => AppNotificationUiConfig.Success.Foreground,
                AppNotificationType.Error => AppNotificationUiConfig.Error.Foreground,
                _ => throw new ArgumentOutOfRangeException()
            };

        return AppNotificationUiConfig.Default.Foreground;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}