using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Buttons;
/// <summary>
/// Interaction logic for Pagination.xaml
/// </summary>
public partial class Pagination : UserControl
{
    public static readonly RoutedEvent PageUpdateEvent = EventManager.RegisterRoutedEvent(nameof(PageUpdate),
        RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));

    public event RoutedEventHandler PageUpdate
    {
        add => AddHandler(PageUpdateEvent, value);
        remove => RemoveHandler(PageUpdateEvent, value);
    }

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(Pagination), new PropertyMetadata(0));

    public int LastPage
    {
        get => (int)GetValue(LastPageProperty);
        set => SetValue(LastPageProperty, value);
    }

    // Using a DependencyProperty as the backing store for LastPage.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LastPageProperty =
        DependencyProperty.Register(nameof(LastPage), typeof(int), typeof(Pagination), new PropertyMetadata(0));

    public Pagination()
    {
        InitializeComponent();
    }

    private void OnPreviousPageClick(object sender, RoutedEventArgs e)
    {
        CurrentPage -= 1;
        RaiseEvent(new RoutedEventArgs(PageUpdateEvent, this));
    }

    private void OnNextPageClick(object sender, RoutedEventArgs e)
    {
        CurrentPage += 1;
        RaiseEvent(new RoutedEventArgs(PageUpdateEvent, this));
    }
}
