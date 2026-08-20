using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Features.Statistics.Models;
using System;
using QuestLevelType = HunterPie.Core.Game.Entity.Game.Quest.QuestLevel;

namespace HunterPie.Features.Statistics.ViewModels;

internal class MockQuestStatisticsSummaryViewModel : QuestStatisticsSummaryViewModel
{
    private static readonly MonsterSummaryViewModel[] MonsterVms =
    {
        new() { GameType = GameType.Rise, Id = 64, IsTarget = true, HuntType = MonsterHuntType.Slay},
        new() { GameType = GameType.Rise, Id = 64, IsTarget = false, HuntType = MonsterHuntType.None },
        new() { GameType = GameType.Rise, Id = 64, IsTarget = true, HuntType = MonsterHuntType.Capture }
    };

    public MockQuestStatisticsSummaryViewModel()
    {
        QuestName = "A Visitor from Eorzea (Extreme)";
        QuestLevel = QuestLevelType.MasterRank;
        Stars = 5;
        QuestType = "Slay";
        QuestTime = TimeSpan.FromSeconds(262);
        GameType = GameType.Rise;
        UploadedAt = DateTime.Now;
        Monsters.Replace(MonsterVms);
    }
}