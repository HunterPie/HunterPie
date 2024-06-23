using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
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
}
