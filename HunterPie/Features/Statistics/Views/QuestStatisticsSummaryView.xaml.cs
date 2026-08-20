using HunterPie.Features.Statistics.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Views;

namespace HunterPie.Features.Statistics.Views;
/// <summary>
/// Interaction logic for QuestStatisticsSummary.xaml
/// </summary>
[View<QuestStatisticsSummaryViewModel>]
public partial class QuestStatisticsSummaryView : ClickableControl
{
    public QuestStatisticsSummaryView()
    {
        InitializeComponent();
    }
}