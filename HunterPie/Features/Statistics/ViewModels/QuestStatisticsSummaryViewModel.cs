using HunterPie.Core.Architecture.Collections;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using GameType = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.Features.Statistics.ViewModels;

public class QuestStatisticsSummaryViewModel : ViewModel
{
    public string? UploadId { get; }
    public string? QuestName { get; set => SetValue(ref field, value); }
    public QuestLevel? QuestLevel { get; set => SetValue(ref field, value); }
    public int? Stars { get; set => SetValue(ref field, value); }
    public string? QuestType { get; set => SetValue(ref field, value); }
    public int Deaths { get; set => SetValue(ref field, value); }
    public int MaxDeaths { get; set => SetValue(ref field, value); }
    public TimeSpan? QuestTime { get; set => SetValue(ref field, value); }
    public GameType GameType
    {
        get;
        set => SetValue(ref field, value);
    }
    public DateTime UploadedAt
    {
        get;
        set => SetValue(ref field, value);
    }

    public ObservableCollectionRange<MonsterSummaryViewModel> Monsters { get; } = new();

    public QuestStatisticsSummaryViewModel() { }

    internal QuestStatisticsSummaryViewModel(
        PoogieQuestSummaryModel model,
        ILocalizationRepository localizationRepository)
    {
        UploadId = model.Id;
        GameType = model.GameType.ToEntity();
        UploadedAt = model.CreatedAt.ToLocalTime();
        QuestTime = model.ElapsedTime;

        IEnumerable<MonsterSummaryViewModel> monsterVms =
            model.Monsters.Select(it => new MonsterSummaryViewModel(GameType, it));

        Monsters.Replace(monsterVms);

        if (model.QuestDetails is not { } details)
            return;

        QuestName = localizationRepository.GetQuestNameBy(GameType, details.Id);
        Deaths = details.Deaths;
        MaxDeaths = details.MaxDeaths;
        QuestType = localizationRepository.FindByEnum(details.Type).String;
        QuestLevel = details.Level;
        Stars = details.Stars;
    }
}