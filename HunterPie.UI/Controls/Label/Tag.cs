using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Label;

public class Tag : Control
{
    public string Label
    {
        get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value);
    }
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(Tag));

    static Tag()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Tag), new FrameworkPropertyMetadata(typeof(Tag)));
    }
}