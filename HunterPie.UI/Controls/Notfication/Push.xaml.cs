using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AppResources = HunterPie.UI.Assets.Application.Resources;

namespace HunterPie.UI.Controls.Notfication;

/// <summary>
/// Interaction logic for Push.xaml
/// </summary>
public partial class Push : UserControl
{
    private static readonly Brush _normalForeground = AppResources.Get<Brush>("WHITE_600");
    private static readonly Brush _normalBackground = AppResources.Get<Brush>("GRAY_300");
    private static readonly Brush _errorForeground = AppResources.Get<Brush>("RED_LIGHT_200");
    private static readonly Brush _errorBackground = AppResources.Get<Brush>("RED_700");
    private static readonly ImageSource _errorIcon = AppResources.Icon("ICON_ERROR_MASK");
    private static readonly Brush _successForeground = AppResources.Get<Brush>("GREEN_LIGHT_300");
    private static readonly Brush _successBackground = AppResources.Get<Brush>("GREEN_700");
    private static readonly ImageSource _successIcon = AppResources.Icon("ICON_LOCK_CHECK_MASK");

    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static new readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register("Background", typeof(Brush), typeof(Push));

    public new Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static new readonly DependencyProperty ForegroundProperty =
        DependencyProperty.Register("Foreground", typeof(Brush), typeof(Push));

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register("Message", typeof(string), typeof(Push));

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(ImageSource), typeof(Push));

    public Push()
    {
        InitializeComponent();
        DataContext = this;
    }

    /// <summary>
    /// Creates a new error notification
    /// </summary>
    /// <param name="message">Message to be displayed in the notification</param>
    /// <returns>Error notification</returns>
    public static Push Error(string message)
    {
        return new Push
        {
            Message = message,
            Background = _errorBackground,
            Foreground = _errorForeground,
            Icon = _errorIcon
        };
    }

    public static Push Success(string message)
    {
        return new Push
        {
            Message = message,
            Background = _successBackground,
            Foreground = _successForeground,
            Icon = _successIcon
        };
    }

    /// <summary>
    /// Creates a new default notification
    /// </summary>
    /// <param name="message">Message to be displayed in the notification</param>
    /// <returns>Notification</returns>
    public static Push Show(string message)
    {
        return new Push
        {
            Message = message,
            Background = _normalBackground,
            Foreground = _normalForeground
        };
    }
}
