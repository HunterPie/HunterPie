using HunterPie.Features.Extensions.ViewModels;
using HunterPie.UI.Architecture.Views;
using System.Windows;

namespace HunterPie.Features.Extensions.Views;

/// <summary>
/// Interaction logic for InstalledPluginView.xaml
/// </summary>
[View<InstalledPluginViewModel>]
public partial class InstalledPluginView
{
    public InstalledPluginView()
    {
        InitializeComponent();
    }

    private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel.NavigateToSettings();
    }
}