using HunterPie.Features.Theme.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace HunterPie.Features.Theme.Views;
/// <summary>
/// Interaction logic for InstalledThemeView.xaml
/// </summary>
public partial class InstalledThemeView
{
    private InstalledThemeViewModel ViewModel => (InstalledThemeViewModel)DataContext;

    public static readonly RoutedEvent BeginDragEvent = EventManager.RegisterRoutedEvent(
        nameof(BeginDrag),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(InstalledThemeView)
    );

    public event RoutedEventHandler BeginDrag
    {
        add => AddHandler(BeginDragEvent, value);
        remove => RemoveHandler(BeginDragEvent, value);
    }

    public InstalledThemeView()
    {
        InitializeComponent();
    }

    private void OnDragButtonDown(object sender, MouseButtonEventArgs e) => RaiseEvent(new RoutedEventArgs(BeginDragEvent, this));

    private void OnEnableTheme(object sender, RoutedEventArgs e)
    {
        ViewModel.Toggle();
    }
}
