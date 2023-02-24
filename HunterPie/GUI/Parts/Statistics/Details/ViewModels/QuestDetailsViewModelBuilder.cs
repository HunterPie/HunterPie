using HunterPie.Integrations.Poogie.Statistics.Models;
using System;
using System.Linq;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;
internal class QuestDetailsViewModelBuilder
{

    public static QuestDetailsViewModel From(PoogieQuestStatisticsModel model)
    {
        var quest = new QuestDetailsViewModel();

        MonsterDetailsViewModel[] monsters = model.Monsters.Select(it => ToMonsterDetails(model, it)).ToArray();

        foreach (MonsterDetailsViewModel monster in monsters)
            quest.Monsters.Add(monster);

        return quest;
    }

    private static MonsterDetailsViewModel ToMonsterDetails(
        PoogieQuestStatisticsModel quest,
        PoogieMonsterStatisticsModel monster
    )
    {
        throw new NotImplementedException();
    }
}
