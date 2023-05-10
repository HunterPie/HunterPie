using HunterPie.Features.Notification.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.Features.Notification.Presentation;
internal class AppNotificationTypeToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AppNotificationType type)
            return type switch
            {
                AppNotificationType.Default => AppNotificationUiConfig.Default.Icon,
                AppNotificationType.Info => AppNotificationUiConfig.Info.Icon,
                AppNotificationType.Success => AppNotificationUiConfig.Success.Icon,
                AppNotificationType.Error => AppNotificationUiConfig.Error.Icon,
                _ => throw new ArgumentOutOfRangeException()
            };

        return AppNotificationUiConfig.Default.Icon;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}