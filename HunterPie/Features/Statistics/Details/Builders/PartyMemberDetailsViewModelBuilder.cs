using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.Features.Statistics.Models;
using HunterPie.UI.Architecture.Brushes;
using LiveCharts.Wpf;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.Details.Builders;

internal class PartyMemberDetailsViewModelBuilder
{
    public static PartyMemberDetailsViewModel? Build(
        HuntStatisticsModel quest,
        MonsterModel monster,
        PartyMemberModel player
    )
    {
        if (monster.HuntStartedAt is not { } startedAt || monster.HuntFinishedAt is not { } finishedAt)
            return null;

        double totalPartyDamage = quest.Players.SelectMany(it => it.Damages)
            .Where(it => it.DealtAt.IsBetween(startedAt, finishedAt))
            .Sum(it => it.Damage);

        var abnormalities =
            player.Abnormalities.Select(it => BuildAbnormality(quest, monster, it))
                                .FilterNull()
                                .ToObservableCollection();
        Color color = RandomColor();

        Brush brush = new SolidColorBrush(color);
        brush.Freeze();

        var damageList = player.Damages.Where(it => it.DealtAt.IsBetween(startedAt, finishedAt))
            .ToImmutableArray();
        float accumulatedDamage = damageList.Sum(it => it.Damage);

        return new PartyMemberDetailsViewModel(quest, monster, damageList, color)
        {
            Name = player.Name,
            Weapon = player.Weapon,
            Damage = accumulatedDamage,
            Contribution = accumulatedDamage / totalPartyDamage,
            Color = brush,
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

        AbnormalityDefinition? schema = AbnormalityRepository.FindBy(quest.Game, abnormality.Id);

        if (schema is not { } abnormalityData)
            return null;

        double questTime = (finishedAt - startedAt).TotalSeconds;

        var timeFrames = abnormality.Activations
            .Where(it => (it.StartedAt >= startedAt || it.FinishedAt >= startedAt) && it.StartedAt < finishedAt)
            .ToImmutableArray();

        double upTime = timeFrames.Sum(it => (it.FinishedAt - it.StartedAt.Max(startedAt)).TotalSeconds);

        Color color = RandomColor();

        var activations = timeFrames.Select(it => new AxisSection
        {
            StrokeThickness = 1,
            StrokeDashArray = new DoubleCollection { 4, 4 },
            Stroke = new SolidColorBrush(color) { Opacity = 0.60 },
            Fill = new SolidColorBrush(color) { Opacity = 0.35 },
            Value = (it.StartedAt - quest.StartedAt).TotalSeconds,
            SectionWidth = (it.FinishedAt - it.StartedAt).TotalSeconds,
        }).ToList();

        return new AbnormalityDetailsViewModel
        {
            Name = abnormalityData.Name,
            Icon = abnormalityData.Icon,
            Color = ColorFadeGradient.FromColor(color),
            UpTime = Math.Min(1.0, upTime / questTime),
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