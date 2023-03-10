using HunterPie.Core.Extensions;
using HunterPie.Features.Statistics.Models;
using HunterPie.UI.Architecture.Adapter;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;
internal class QuestDetailsViewModelBuilder
{

    public static async Task<QuestDetailsViewModel> From(HuntStatisticsModel model)
    {
        var quest = new QuestDetailsViewModel();

        MonsterDetailsViewModel[] monsters = model.Monsters
            .OrderByDescending(it => it.Enrage.Activations.Length)
            .Select(async it => await ToMonsterDetails(model, it))
            .Select(it => it.Result)
            .ToArray();

        foreach (MonsterDetailsViewModel monster in monsters)
            quest.Monsters.Add(monster);

        return quest;
    }

    private static async Task<MonsterDetailsViewModel> ToMonsterDetails(
        HuntStatisticsModel quest,
        MonsterModel monster
    )
    {
        TimeSpan? timeElapsed = null;
        if (monster.HuntStartedAt is { } startedAt && monster.HuntFinishedAt is { } finishedAt)
            timeElapsed = finishedAt - startedAt;

        return new MonsterDetailsViewModel
        {
            Name = MonsterNameAdapter.From(quest.Game, monster.Id),
            Icon = await MonsterIconAdapter.UriFrom(quest.Game, monster.Id),
            HuntedAt = monster.HuntStartedAt ?? DateTime.MinValue,
            MaxHealth = monster.MaxHealth,
            TimeElapsed = timeElapsed,
            Statuses = { BuildEnrage(timeElapsed ?? TimeSpan.Zero, monster.Enrage) },
            Players = quest.Players.Select(it => BuildPlayer(monster, it))
                                   .Where(it => it != null)
                                   .ToObservableCollection(),
            Crown = monster.Crown,
        };
    }

    private static StatusDetailsViewModel BuildEnrage(
        TimeSpan huntTimeElapsed,
        MonsterStatusModel status
    )
    {
        double timeElapsed = huntTimeElapsed.TotalSeconds;

        double activationsTotalSeconds = status.Activations.Select(it => it.FinishedAt - it.StartedAt)
            .Sum(it => it.TotalSeconds);

        return new StatusDetailsViewModel
        {
            Color = Brushes.Red,
            Name = "Enrage",
            UpTime = activationsTotalSeconds / Math.Max(1.0, timeElapsed)
        };
    }

    private static PartyMemberDetailsViewModel? BuildPlayer(
        MonsterModel monster,
        PartyMemberModel player
    )
    {
        if (monster.HuntStartedAt is not { } startedAt || monster.HuntFinishedAt is not { } finishedAt)
            return null;

        PlayerDamageFrameModel[] damageFrames = player.Damages.Where(it => it.DealtAt >= startedAt
                                                                           && it.DealtAt <= finishedAt).ToArray();

        return new PartyMemberDetailsViewModel
        {
            Name = player.Name,
            Weapon = player.Weapon,
            // TODO: Add damage series
        };
    }
}
