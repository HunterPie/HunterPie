using HunterPie.Features.Notification.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.Features.Notification.Presentation;
internal class AppNotificationTypeToAccentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AppNotificationType type)
            return type switch
            {
                AppNotificationType.Default => AppNotificationUiConfig.Default.Accent,
                AppNotificationType.Info => AppNotificationUiConfig.Info.Accent,
                AppNotificationType.Success => AppNotificationUiConfig.Success.Accent,
                AppNotificationType.Error => AppNotificationUiConfig.Error.Accent,
                _ => throw new ArgumentOutOfRangeException()
            };

        return AppNotificationUiConfig.Default.Accent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
