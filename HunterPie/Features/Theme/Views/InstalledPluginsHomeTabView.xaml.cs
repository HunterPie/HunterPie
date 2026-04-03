using HunterPie.Features.Theme.ViewModels;
using System.Windows;

namespace HunterPie.Features.Theme.Views;

/// <summary>
/// Interaction logic for InstalledPluginsHomeTabView.xaml
/// </summary>
public partial class InstalledPluginsHomeTabView
{
    private InstalledPluginsHomeTabViewModel ViewModel => (InstalledPluginsHomeTabViewModel)DataContext;

    public InstalledPluginsHomeTabView()
    {
        InitializeComponent();
    }

    private async void OnRefreshButtonClicked(object sender, RoutedEventArgs e)
    {
        await ViewModel.RefreshAsync();
    }
}
