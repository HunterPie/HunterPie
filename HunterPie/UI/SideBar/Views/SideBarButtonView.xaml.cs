using HunterPie.UI.Architecture.Views;
using HunterPie.UI.Client.Sidebar.Handler;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.SideBar.Views;

/// <summary>
/// Interaction logic for SideBarButtonView.xaml
/// </summary>
[View<NavigationHandler>]
public partial class SideBarButtonView : UserControl
{
    public SideBarButtonView()
    {
        InitializeComponent();
    }

    private async void OnClick(object sender, RoutedEventArgs e)
        => await ViewModel.ExecuteAsync();
}