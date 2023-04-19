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
    // TODO: Remove all this messy stuff from here and make UI agnostic

    private static readonly Brush NormalForeground = AppResources.Get<Brush>("WHITE_600");
    private static readonly Brush NormalBackground = AppResources.Get<Brush>("GRAY_300");
    private static readonly Brush ErrorForeground = AppResources.Get<Brush>("RED_LIGHT_200");
    private static readonly Brush ErrorBackground = AppResources.Get<Brush>("RED_700");
    private static readonly ImageSource ErrorIcon = AppResources.Icon("ICON_ERROR_MASK");
    private static readonly Brush SuccessForeground = AppResources.Get<Brush>("GREEN_LIGHT_300");
    private static readonly Brush SuccessBackground = AppResources.Get<Brush>("GREEN_700");
    private static readonly ImageSource SuccessIcon = AppResources.Icon("ICON_CHECKMARK");
    private static readonly Brush InfoForeground = AppResources.Get<Brush>("BLUE_LIGHT_400");
    private static readonly Brush InfoBackground = AppResources.Get<Brush>("BLUE_700");
    private static readonly ImageSource InfoIcon = AppResources.Icon("ICON_INFO");

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
            Background = ErrorBackground,
            Foreground = ErrorForeground,
            Icon = ErrorIcon
        };
    }

    public static Push Success(string message)
    {
        return new Push
        {
            Message = message,
            Background = SuccessBackground,
            Foreground = SuccessForeground,
            Icon = SuccessIcon
        };
    }

    public static Push Info(string message)
    {
        return new Push
        {
            Message = message,
            Background = InfoBackground,
            Foreground = InfoForeground,
            Icon = InfoIcon
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
            Background = NormalBackground,
            Foreground = NormalForeground
        };
    }
}
