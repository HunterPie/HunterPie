using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;

public class QuestStatisticsSummariesViewModel : ViewModel
{
    private const int MAX_PER_PAGE = 5;
    private PoogieQuestSummaryModel[] _summaries = Array.Empty<PoogieQuestSummaryModel>();
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

    private int _currentPage;
    public int CurrentPage
    {
        get => _currentPage;
        set => SetValue(ref _currentPage, value);
    }

    private int _lastPage;
    public int LastPage
    {
        get => _lastPage;
        set => SetValue(ref _lastPage, value);
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

        _summaries = summaries.OrderByDescending(it => it.CreatedAt)
            .ToArray();

        LastPage = summaries.Count / MAX_PER_PAGE;

        UpdateSummariesContainer();
    }

    public void RequestPageUpdate() => UpdateSummariesContainer();

    private void UpdateSummariesContainer()
    {
        Summaries.Clear();

        foreach (PoogieQuestSummaryModel summary in _summaries.Skip(CurrentPage * MAX_PER_PAGE).Take(MAX_PER_PAGE))
            Summaries.Add(new QuestStatisticsSummaryViewModel(summary));
    }
}