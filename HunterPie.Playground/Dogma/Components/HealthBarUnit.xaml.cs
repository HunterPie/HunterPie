using System.Windows;

namespace HunterPie.Playground.Dogma.Components;
/// <summary>
/// Interaction logic for HealthBarUnit.xaml
/// </summary>
public partial class HealthBarUnit
{
    public bool IsFull { get => (bool)GetValue(IsFullProperty); set => SetValue(IsFullProperty, value); }

    // Using a DependencyProperty as the backing store for IsFull.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsFullProperty =
        DependencyProperty.Register(nameof(IsFull), typeof(bool), typeof(HealthBarUnit), new PropertyMetadata(false));

    public HealthBarUnit()
    {
        InitializeComponent();
    }
}
