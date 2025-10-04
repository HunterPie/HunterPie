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

    public static readonly RoutedEvent ToggleEvent = EventManager.RegisterRoutedEvent(
        nameof(Toggle),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(InstalledThemeView)
    );

    public event RoutedEventHandler Toggle
    {
        add => AddHandler(ToggleEvent, value);
        remove => RemoveHandler(ToggleEvent, value);
    }

    public InstalledThemeView()
    {
        InitializeComponent();
    }

    private void OnDragButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!ViewModel.IsEnabled)
            return;

        RaiseEvent(new RoutedEventArgs(BeginDragEvent, this));
    }

    private void OnEnableTheme(object sender, RoutedEventArgs e)
    {
        ViewModel.Toggle();

        RaiseEvent(new RoutedEventArgs(ToggleEvent, this));
    }

    private void OnOpenFolderClick(object sender, RoutedEventArgs e)
    {
        ViewModel.OpenFolder();
    }
}