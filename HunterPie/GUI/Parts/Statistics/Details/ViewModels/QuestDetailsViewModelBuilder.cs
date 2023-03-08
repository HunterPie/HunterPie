using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture.Adapter;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;
internal class QuestDetailsViewModelBuilder
{

    public static async Task<QuestDetailsViewModel> From(PoogieQuestStatisticsModel model)
    {
        var quest = new QuestDetailsViewModel();

        MonsterDetailsViewModel[] monsters = model.Monsters.Select(async it => await ToMonsterDetails(model, it))
            .Select(it => it.Result)
            .ToArray();

        foreach (MonsterDetailsViewModel monster in monsters)
            quest.Monsters.Add(monster);

        return quest;
    }

    private static async Task<MonsterDetailsViewModel> ToMonsterDetails(
        PoogieQuestStatisticsModel quest,
        PoogieMonsterStatisticsModel monster
    )
    {
        TimeSpan? timeElapsed = null;
        if (monster.HuntStartedAt is { } startedAt && monster.HuntFinishedAt is { } finishedAt)
            timeElapsed = finishedAt - startedAt;

        return new MonsterDetailsViewModel
        {
            Name = MonsterNameAdapter.From(quest.GameType.ToEntity(), monster.Id),
            Icon = await MonsterIconAdapter.UriFrom(quest.GameType.ToEntity(), monster.Id),
            HuntedAt = monster.HuntStartedAt ?? DateTime.MinValue,
            MaxHealth = monster.MaxHealth,
            TimeElapsed = timeElapsed,
            Crown = monster.Crown,
        };
    }
}
