using HunterPie.GUI.Parts.Statistics.ViewModels;
using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Statistics.Views;
/// <summary>
/// Interaction logic for QuestStatisticsSummary.xaml
/// </summary>
public partial class QuestStatisticsSummaryView : ClickableControl, IView<QuestStatisticsSummaryViewModel>
{
    public QuestStatisticsSummaryViewModel ViewModel => (QuestStatisticsSummaryViewModel)DataContext;

    public QuestStatisticsSummaryView()
    {
        InitializeComponent();
    }
}
