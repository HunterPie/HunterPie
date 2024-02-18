using HunterPie.Core.Client.Configuration.Enums;
using System;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;

public class MockQuestStatisticsSummaryViewModel : QuestStatisticsSummaryViewModel
{
    private static readonly MonsterSummaryViewModel[] MonsterVms =
    {
        new MonsterSummaryViewModel { GameType = GameType.Rise, Id = 64, IsTarget = true },
        new MonsterSummaryViewModel { GameType = GameType.Rise, Id = 64, IsTarget = false },
        new MonsterSummaryViewModel { GameType = GameType.Rise, Id = 64, IsTarget = true }
    };

    public MockQuestStatisticsSummaryViewModel()
    {
        QuestName = "A Visitor from Eorzea (Extreme)";
        QuestLevel = 9;
        QuestType = "Slay";
        QuestTime = TimeSpan.FromSeconds(262);
        GameType = GameType.Rise;
        UploadedAt = DateTime.Now;
        Monsters.Replace(MonsterVms);
    }
}