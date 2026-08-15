using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Events;
using HunterPie.UI.Architecture.Views;
using HunterPie.UI.Controls.Popup.Events;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using System.Windows;

namespace HunterPie.UI.Controls.Settings.Monsters.Views;
/// <summary>
/// Interaction logic for MonsterConfigurationsActivity.xaml
/// </summary>
[View<MonsterConfigurationsViewModel>]
public partial class MonsterConfigurationsActivity : Activity
{
    public MonsterConfigurationsActivity()
    {
        InitializeComponent();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ViewModel.FetchIcons();
    }

    private void OnOverrideMonsterClick(object sender, RoutedEventArgs e)
    {
        ViewModel.IsSearching = !ViewModel.IsSearching;
    }

    private void OnSelectMonsterClick(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: MonsterConfigurationViewModel configurationViewModel })
            return;

        ViewModel.CreateOverride(configurationViewModel);
    }

    private void OnMonsterDeleteClick(object sender, DataRoutedEventArgs<MonsterConfigurationViewModel> e)
    {
        MonsterConfigurationViewModel configurationViewModel = e.Data;

        ViewModel.RemoveOverride(configurationViewModel);
    }

    private void OnSearch(object sender, RoutedSearchEventArgs e)
    {
        ViewModel.FilterQuery(e.Query);
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel.Return();
    }
}