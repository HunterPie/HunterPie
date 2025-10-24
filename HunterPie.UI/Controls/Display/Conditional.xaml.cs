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
        DependencyProperty.Register(nameof(Condition), typeof(bool), typeof(Conditional), new PropertyMetadata(false, OnPropertyChanged));

    public DataTemplate Component
    {
        get => (DataTemplate)GetValue(ComponentProperty);
        set => SetValue(ComponentProperty, value);
    }
    public static readonly DependencyProperty ComponentProperty =
        DependencyProperty.Register(nameof(Component), typeof(DataTemplate), typeof(Conditional), new PropertyMetadata(null, OnPropertyChanged));

    public DataTemplate Otherwise
    {
        get => (DataTemplate)GetValue(OtherwiseProperty);
        set => SetValue(OtherwiseProperty, value);
    }
    public static readonly DependencyProperty OtherwiseProperty =
        DependencyProperty.Register(nameof(Otherwise), typeof(DataTemplate), typeof(Conditional), new PropertyMetadata(null, OnPropertyChanged));

    private DataTemplate CurrentTemplate
    {
        get => (DataTemplate)GetValue(CurrentTemplateProperty);
        set => SetValue(CurrentTemplateProperty, value);
    }
    private static readonly DependencyProperty CurrentTemplateProperty =
        DependencyProperty.Register(nameof(CurrentTemplate), typeof(DataTemplate), typeof(Conditional), new PropertyMetadata(null, OnPropertyChanged));

    public Conditional()
    {
        InitializeComponent();
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Conditional conditional)
            return;

        conditional.CurrentTemplate = conditional.Condition
            ? conditional.Component
            : conditional.Otherwise;
    }
}