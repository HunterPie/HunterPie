using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
public class QuestStatisticsSummaryViewModel : ViewModel
{
    private readonly PoogieStatisticsConnector _connector = new();

    public ObservableCollection<MonsterSummaryViewModel> Monsters { get; } = new();

    public async void FetchQuests()
    {
        _connector.
    }
}
