using HunterPie.UI.Assets.Application;
using System.Windows.Media;

namespace HunterPie.Features.Notification.Presentation;

internal static class AppNotificationUiConfig
{
    public record NotificationUi(
        Brush Foreground,
        Brush Accent,
        ImageSource Icon
    );

    public static readonly NotificationUi Default = new NotificationUi(
        Foreground: Resources.Get<Brush>("WHITE_500"),
        Accent: Resources.Get<Brush>("GRAY_50"),
        Icon: Resources.Icon("ICON_INFO")
    );

    public static readonly NotificationUi Info = new NotificationUi(
        Foreground: Resources.Get<Brush>("BLUE_LIGHT_200"),
        Accent: Resources.Get<Brush>("BLUE_400"),
        Icon: Resources.Icon("ICON_INFO")
    );

    public static readonly NotificationUi Error = new NotificationUi(
        Foreground: Resources.Get<Brush>("RED_LIGHT_200"),
        Accent: Resources.Get<Brush>("RED_400"),
        Icon: Resources.Icon("ICON_ERROR_MASK")
    );

    public static readonly NotificationUi Success = new NotificationUi(
        Foreground: Resources.Get<Brush>("GREEN_LIGHT_200"),
        Accent: Resources.Get<Brush>("GREEN_300"),
        Icon: Resources.Icon("ICON_CHECKMARK")
    );

}
