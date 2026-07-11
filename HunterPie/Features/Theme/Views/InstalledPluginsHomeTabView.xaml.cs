using HunterPie.Features.Theme.ViewModels;
using HunterPie.UI.Architecture.Views;
using System.Windows;

namespace HunterPie.Features.Theme.Views;

/// <summary>
/// Interaction logic for InstalledPluginsHomeTabView.xaml
/// </summary>
[View<InstalledPluginsHomeTabViewModel>]
public partial class InstalledPluginsHomeTabView
{
    public InstalledPluginsHomeTabView()
    {
        InitializeComponent();
    }

    private async void OnRefreshButtonClicked(object sender, RoutedEventArgs e)
    {
        await ViewModel.RefreshAsync();
    }

    private void OnOpenFolderButtonClicked(object sender, RoutedEventArgs e)
    {
        ViewModel.OpenFolder();
    }
}