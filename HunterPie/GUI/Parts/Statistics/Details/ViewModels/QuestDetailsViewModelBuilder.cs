using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Services;
using HunterPie.Features.Statistics.Models;
using HunterPie.UI.Architecture.Adapter;
using HunterPie.UI.Architecture.Brushes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
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

        StatusDetailsViewModel? enrage = BuildEnrage(quest, huntElapsed ?? TimeSpan.Zero, monster.Enrage);

        var statuses = new ObservableCollection<StatusDetailsViewModel>();

        if (enrage != null)
            statuses.Add(enrage);

        return new MonsterDetailsViewModel
        {
            Name = MonsterNameAdapter.From(quest.Game, monster.Id),
            Icon = await MonsterIconAdapter.UriFrom(quest.Game, monster.Id),
            TimeElapsed = quest.FinishedAt - quest.StartedAt,
            MaxHealth = monster.MaxHealth,
            HuntElapsed = huntElapsed,
            Statuses = statuses,
            Players = quest.Players.Select(it => BuildPlayer(quest, monster, it))
                                   .Where(it => it != null)
                                   .ToObservableCollection(),
            Crown = monster.Crown,
        };
    }

    private static StatusDetailsViewModel? BuildEnrage(
        HuntStatisticsModel quest,
        TimeSpan huntTimeElapsed,
        MonsterStatusModel status
    )
    {
        double timeElapsed = huntTimeElapsed.TotalSeconds;

        double activationsTotalSeconds = status.Activations.Select(it => it.FinishedAt - it.StartedAt)
            .Sum(it => it.TotalSeconds);

        var activations = status.Activations.Select(it => new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection { 4, 4 },
            Value = (it.StartedAt - quest.StartedAt).TotalSeconds,
            SectionWidth = (it.FinishedAt - it.StartedAt).TotalSeconds
        }).ToList();

        return new StatusDetailsViewModel
        {
            Color = Brushes.Red,
            Name = "Enrage",
            UpTime = activationsTotalSeconds / Math.Max(1.0, timeElapsed),
            Activations = activations
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

        var abnormalities =
            player.Abnormalities.Select(it => BuildAbnormality(quest, monster, it))
                .Where(it => it != null)
                .ToObservableCollection();

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
                PointGeometry = null,
                StrokeThickness = 2,
                LineSmoothness = 1,
                Values = damagePoints
            },
            Color = new SolidColorBrush(color),
            Abnormalities = abnormalities
        };
    }

    private static AbnormalityDetailsViewModel? BuildAbnormality(
        HuntStatisticsModel quest,
        MonsterModel monster,
        AbnormalityModel abnormality
    )
    {
        if (monster.HuntFinishedAt is not { } finishedAt || monster.HuntStartedAt is not { } startedAt)
            return null;

        AbnormalitySchema? schema = AbnormalityService.FindBy(quest.Game, abnormality.Id);

        if (schema is not { } abnormalityData)
            return null;

        double questTime = (finishedAt - startedAt).TotalSeconds;

        var timeFrames = abnormality.Activations
            .Where(it => (it.StartedAt >= startedAt || it.FinishedAt >= startedAt) && it.StartedAt < finishedAt)
            .ToImmutableArray();

        double upTime = timeFrames.Sum(it => (it.FinishedAt - it.StartedAt).TotalSeconds) / questTime;

        Color color = RandomColor() with { A = 90 };

        var activations = timeFrames.Select(it => new AxisSection
        {
            StrokeThickness = 0,
            Fill = new SolidColorBrush(color),
            Value = (it.StartedAt - quest.StartedAt).TotalSeconds,
            SectionWidth = (it.FinishedAt - it.StartedAt).TotalSeconds,
        }).ToList();

        return new AbnormalityDetailsViewModel
        {
            Name = abnormalityData.Name,
            Icon = abnormalityData.Icon,
            Color = new SolidColorBrush(color),
            UpTime = Math.Min(1.0, upTime),
            Activations = activations
        };
    }

    private static Color RandomColor()
    {
        Random rng = new();
        byte r = (byte)rng.Next(115, 256);
        byte g = (byte)rng.Next(180, 256);
        byte b = (byte)rng.Next(120, 256);
        return Color.FromRgb(r, g, b);
    }
}
