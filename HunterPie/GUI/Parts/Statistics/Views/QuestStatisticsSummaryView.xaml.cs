using HunterPie.GUI.Parts.Statistics.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Statistics.Views;
/// <summary>
/// Interaction logic for QuestStatisticsSummary.xaml
/// </summary>
public partial class QuestStatisticsSummaryView : UserControl
{
    private QuestStatisticsSummaryViewModel ViewModel => (QuestStatisticsSummaryViewModel)DataContext;

    public QuestStatisticsSummaryView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e) =>
        ViewModel.NavigateToHuntDetails();
}
