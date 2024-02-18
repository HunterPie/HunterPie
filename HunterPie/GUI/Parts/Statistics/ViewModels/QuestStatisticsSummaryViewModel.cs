using HunterPie.Core.Architecture.Collections;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using GameType = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
public class QuestStatisticsSummaryViewModel : ViewModel
{
    public string? UploadId { get; }

    private string? _questName;
    public string? QuestName { get => _questName; set => SetValue(ref _questName, value); }

    private int? _questLevel;
    public int? QuestLevel { get => _questLevel; set => SetValue(ref _questLevel, value); }

    private string? _questType;
    public string? QuestType { get => _questType; set => SetValue(ref _questType, value); }

    private int _deaths;
    public int Deaths { get => _deaths; set => SetValue(ref _deaths, value); }

    private int _maxDeaths;
    public int MaxDeaths { get => _maxDeaths; set => SetValue(ref _maxDeaths, value); }

    private TimeSpan? _questTime;
    public TimeSpan? QuestTime { get => _questTime; set => SetValue(ref _questTime, value); }

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

    public ObservableCollectionRange<MonsterSummaryViewModel> Monsters { get; } = new();

    public QuestStatisticsSummaryViewModel() { }

    internal QuestStatisticsSummaryViewModel(PoogieQuestSummaryModel model)
    {
        UploadId = model.Id;

        GameType = model.GameType.ToEntity();

        UploadedAt = model.CreatedAt.ToLocalTime();

        IEnumerable<MonsterSummaryViewModel> monsterVms =
            model.Monsters.Select(it => new MonsterSummaryViewModel(GameType, it));

        Monsters.Replace(monsterVms);
    }
}
