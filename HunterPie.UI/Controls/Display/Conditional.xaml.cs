using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Display;
/// <summary>
/// Interaction logic for Conditional.xam
/// </summary>
public partial class Conditional : UserControl
{

    public bool Condition
    {
        get => (bool)GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }
    public static readonly DependencyProperty ConditionProperty =
        DependencyProperty.Register(nameof(Condition), typeof(bool), typeof(Conditional));

    public object Component
    {
        get => GetValue(ComponentProperty);
        set => SetValue(ComponentProperty, value);
    }
    public static readonly DependencyProperty ComponentProperty =
        DependencyProperty.Register(nameof(Component), typeof(object), typeof(Conditional));

    public object Otherwise
    {
        get => GetValue(OtherwiseProperty);
        set => SetValue(OtherwiseProperty, value);
    }
    public static readonly DependencyProperty OtherwiseProperty =
        DependencyProperty.Register(nameof(Otherwise), typeof(object), typeof(Conditional));

    public Conditional()
    {
        InitializeComponent();
    }
}
