using HunterPie.UI.Architecture.Views;
using HunterPie.UI.Client.Sidebar.Handler;
using System.Windows;
using System.Windows.Input;

namespace HunterPie.UI.SideBar.Views;
/// <summary>
/// Interaction logic for FixedSideBarButtonView.xaml
/// </summary>
[View<NavigationHandler>]
public partial class FixedSideBarButtonView
{
    public FixedSideBarButtonView()
    {
        InitializeComponent();
    }

    private async void OnClick(object sender, RoutedEventArgs e)
        => await ViewModel.ExecuteAsync();

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        PART_Popup.IsOpen = true;
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        PART_Popup.IsOpen = false;
    }
}
