using HunterPie.Core.Extensions;
using HunterPie.Features.Statistics.Models;
using HunterPie.UI.Architecture.Adapter;
using HunterPie.UI.Architecture.Brushes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;

#nullable enable
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
        TimeSpan? huntElapsed = null;
        if (monster.HuntStartedAt is { } startedAt && monster.HuntFinishedAt is { } finishedAt)
            huntElapsed = finishedAt - startedAt;

        return new MonsterDetailsViewModel
        {
            Name = MonsterNameAdapter.From(quest.Game, monster.Id),
            Icon = await MonsterIconAdapter.UriFrom(quest.Game, monster.Id),
            TimeElapsed = quest.FinishedAt - quest.StartedAt,
            MaxHealth = monster.MaxHealth,
            HuntElapsed = huntElapsed,
            Statuses = { BuildEnrage(huntElapsed ?? TimeSpan.Zero, monster.Enrage) },
            Players = quest.Players.Select(it => BuildPlayer(quest, monster, it))
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
        HuntStatisticsModel quest,
        MonsterModel monster,
        PartyMemberModel player
    )
    {
        if (monster.HuntStartedAt is not { } startedAt || monster.HuntFinishedAt is not { } finishedAt)
            return null;

        float accumulatedDamage = 0;
        IEnumerable<ObservablePoint> damageFrames = player.Damages.Where(it => it.DealtAt >= startedAt && it.DealtAt <= finishedAt)
            .Select(it => (it.DealtAt, Damage: accumulatedDamage = it.Damage + accumulatedDamage))
            .Select(it =>
              {
                  double time = Math.Max(1.0, (it.DealtAt - quest.StartedAt).TotalSeconds);

                  return new ObservablePoint
                  {
                      X = time,
                      Y = it.Damage / time
                  };
              });

        var damagePoints = new ChartValues<ObservablePoint>(damageFrames);
        Color color = RandomColor();

        return new PartyMemberDetailsViewModel
        {
            Name = player.Name,
            Weapon = player.Weapon,
            Damages = new LineSeries
            {
                Title = player.Name,
                Stroke = new SolidColorBrush(color),
                Fill = ColorFadeGradient.FromColor(color),
                PointGeometrySize = 1,
                StrokeThickness = 2,
                LineSmoothness = 1,
                Values = damagePoints
            },
            Color = new SolidColorBrush(color)
        };
    }

    private static Color RandomColor()
    {
        Random rng = new();
        byte r = (byte)rng.Next(256);
        byte g = (byte)rng.Next(256);
        byte b = (byte)rng.Next(256);
        return Color.FromRgb(r, g, b);
    }
}
