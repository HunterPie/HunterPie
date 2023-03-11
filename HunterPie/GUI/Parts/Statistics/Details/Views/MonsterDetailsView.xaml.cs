using HunterPie.GUI.Parts.Statistics.Details.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Statistics.Details.Views;
/// <summary>
/// Interaction logic for MonsterDetailsView.xaml
/// </summary>
public partial class MonsterDetailsView : UserControl, IView<MonsterDetailsViewModel>
{
    public MonsterDetailsViewModel ViewModel => (MonsterDetailsViewModel)DataContext;

    public MonsterDetailsView()
    {
        InitializeComponent();
    }

    private void OnViewLoaded(object sender, RoutedEventArgs e) => ViewModel.SetupGraph();

    private void OnPlayerClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: PartyMemberDetailsViewModel vm })
            ViewModel.SetGraphTo(vm);
    }
}
