using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;

public class QuestStatisticsSummariesViewModel : ViewModel
{
    private readonly PoogieStatisticsConnector _connector = new();

    private bool _hasQuests;
    public bool HasQuests
    {
        get => _hasQuests;
        set => SetValue(ref _hasQuests, value);
    }

    private bool _isFetchingQuests;
    public bool IsFetchingQuests
    {
        get => _isFetchingQuests;
        set => SetValue(ref _isFetchingQuests, value);
    }

    public ObservableCollection<QuestStatisticsSummaryViewModel> Summaries { get; } = new();

    public async void FetchQuests()
    {
        IsFetchingQuests = true;

        PoogieResult<List<PoogieQuestSummaryModel>> summariesResponse = await _connector.GetUserQuestSummaries();

        IsFetchingQuests = false;

        if (summariesResponse.Response is not { } summaries || !summaries.Any())
        {
            HasQuests = false;
            return;
        }

        foreach (PoogieQuestSummaryModel summary in summaries)
            Summaries.Add(new QuestStatisticsSummaryViewModel(summary));
    }
}