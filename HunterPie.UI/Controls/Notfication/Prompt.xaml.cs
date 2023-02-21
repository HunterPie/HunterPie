using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Notfication;
/// <summary>
/// Interaction logic for Prompt.xaml
/// </summary>
public partial class Prompt : UserControl
{
    public Thickness IconMargin
    {
        get => (Thickness)GetValue(IconMarginProperty);
        set => SetValue(IconMarginProperty, value);
    }

    // Using a DependencyProperty as the backing store for IconMargin.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconMarginProperty =
        DependencyProperty.Register(nameof(IconMargin), typeof(Thickness), typeof(Prompt), new PropertyMetadata(new Thickness(3)));

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(Prompt));

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register(nameof(Message), typeof(string), typeof(Prompt), new PropertyMetadata(string.Empty));



    public Prompt()
    {
        InitializeComponent();
    }
}
