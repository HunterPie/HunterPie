using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;
using GameType = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
public class QuestStatisticsSummaryViewModel : ViewModel
{
    private readonly PoogieStatisticsConnector _connector = new();
    public string? UploadId { get; }

    private GameType _gameType;
    public GameType GameType
    {
        get => _gameType;
        set => SetValue(ref _gameType, value);
    }

    private DateTime _uploadedAt;
    public DateTime UploadedAt
    {
        get => _uploadedAt;
        set => SetValue(ref _uploadedAt, value);
    }

    private bool _isFetchingDetails;
    public bool IsFetchingDetails
    {
        get => _isFetchingDetails;
        set => SetValue(ref _isFetchingDetails, value);
    }

    public ObservableCollection<MonsterSummaryViewModel> Monsters { get; } = new();

    public QuestStatisticsSummaryViewModel() { }

    internal QuestStatisticsSummaryViewModel(PoogieQuestSummaryModel model)
    {
        UploadId = model.Id;

        GameType = model.GameType.ToEntity();

        UploadedAt = model.CreatedAt.ToLocalTime();

        foreach (PoogieMonsterSummaryModel monster in model.Monsters)
            Monsters.Add(new MonsterSummaryViewModel(model.GameType.ToEntity(), monster));
    }
}
