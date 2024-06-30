using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Monsters.Views;
/// <summary>
/// Interaction logic for MonsterConfigurationView.xaml
/// </summary>
public partial class MonsterConfigurationView : UserControl
{
    public MonsterConfigurationView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterConfigurationViewModel vm)
            return;

        vm.IsEditing = !vm.IsEditing;
    }
}
