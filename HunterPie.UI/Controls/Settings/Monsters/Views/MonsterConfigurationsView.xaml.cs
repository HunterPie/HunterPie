using HunterPie.UI.Controls.Popup.Events;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using HunterPie.UI.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Monsters.Views;
/// <summary>
/// Interaction logic for MonsterConfigurationsView.xaml
/// </summary>
public partial class MonsterConfigurationsView : UserControl
{
    public MonsterConfigurationsView()
    {
        InitializeComponent();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterConfigurationsViewModel vm)
            return;

        vm.BindAndUpdateSettings();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterConfigurationsViewModel vm)
            return;

        vm.Dispose();
    }

    private void OnOverrideMonsterClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterConfigurationsViewModel vm)
            return;

        vm.IsSearching = !vm.IsSearching;
    }

    private void OnSelectMonsterClick(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: MonsterConfigurationViewModel vm })
            return;

        vm.IsOverriding = true;
    }

    private void OnSearch(object sender, RoutedSearchEventArgs e)
    {
        if (DataContext is not MonsterConfigurationsViewModel vm)
            return;

        vm.FilterQuery(e.Query);
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        Navigator.Body.Return();
    }
}
