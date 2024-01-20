using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Paginating;
/// <summary>
/// Interaction logic for Pagination.xaml
/// </summary>
public partial class Pagination : UserControl
{
    public int TotalPages
    {
        get => (int)GetValue(TotalPagesProperty);
        set => SetValue(TotalPagesProperty, value);
    }

    public static readonly DependencyProperty TotalPagesProperty =
        DependencyProperty.Register(nameof(TotalPages), typeof(int), typeof(Pagination), new PropertyMetadata(0));

    public Pagination()
    {
        InitializeComponent();
    }
}
