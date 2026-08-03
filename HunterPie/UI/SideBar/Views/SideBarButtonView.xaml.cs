using HunterPie.UI.Architecture.Views;
using HunterPie.UI.Client.Sidebar.Handler;
using System.Windows;
using System.Windows.Input;

namespace HunterPie.UI.SideBar.Views;

/// <summary>
/// Interaction logic for SideBarButtonView.xaml
/// </summary>
[View<NavigationHandler>]
public partial class SideBarButtonView
{
    public bool IsFixed { get => (bool)GetValue(IsFixedProperty); set => SetValue(IsFixedProperty, value); }

    // Using a DependencyProperty as the backing store for IsFixed.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsFixedProperty =
        DependencyProperty.Register(nameof(IsFixed), typeof(bool), typeof(SideBarButtonView), new PropertyMetadata(false));

    public SideBarButtonView()
    {
        InitializeComponent();
    }

    private async void OnClick(object sender, RoutedEventArgs e)
    {
        if (ViewModel is NavigationHandler.Action action)
            await action.ExecuteAsync();
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        PART_Popup.IsOpen = true;
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        PART_Popup.IsOpen = false;
    }
}