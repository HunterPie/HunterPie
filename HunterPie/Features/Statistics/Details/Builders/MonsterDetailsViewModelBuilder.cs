using HunterPie.Core.Extensions;
using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.Features.Statistics.Models;
using HunterPie.UI.Architecture.Adapter;
using HunterPie.UI.Assets.Application;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.Features.Statistics.Details.Builders;
internal static class MonsterDetailsViewModelBuilder
{
    private static readonly Brush EnrageBrush = Resources.Get<Brush>("HUNT_EXPORT_ENRAGE_BRUSH");
    private static readonly Brush HealthStepBrush = Resources.Get<Brush>("WHITE_400");

    [Obsolete]
    public static async Task<MonsterDetailsViewModel> Build(HuntStatisticsModel hunt, MonsterModel monster)
    {
        TimeSpan? huntElapsed = null;
        LineSeries? healthSteps = null;
        if (monster is { HuntStartedAt: { } startedAt, HuntFinishedAt: { } finishedAt })
        {
            huntElapsed = finishedAt - startedAt;

            MonsterHealthStepModel? lastHealthStep = monster.HealthSteps.LastOrDefault();

            if (lastHealthStep is { } lastModel)
                monster.HealthSteps.Add(
                    item: lastModel with { Time = finishedAt }
                );

            IEnumerable<ObservablePoint> healthPoints = monster.HealthSteps.Select(it =>
            {
                TimeSpan huntStartedDelta = startedAt - hunt.StartedAt;
                double capturedAt = (it.Time - hunt.StartedAt + huntStartedDelta).TotalSeconds;
                return new ObservablePoint(
                    x: capturedAt,
                    y: it.Percentage
                );
            });

            var backgroundBrush = new SolidColorBrush(Colors.White)
            {
                Opacity = 0.1
            };
            backgroundBrush.Freeze();

            healthSteps = monster.HealthSteps.Count > 0
                ? new LineSeries
                {
                    StrokeThickness = 1,
                    Stroke = HealthStepBrush,
                    PointForeground = backgroundBrush,
                    Values = new ChartValues<ObservablePoint>(healthPoints),
                    StrokeDashArray = new DoubleCollection(new double[] { 4, 1 }),
                    ScalesYAt = 0,
                    Fill = backgroundBrush
                }
                : null;
        }

        return new MonsterDetailsViewModel
        {
            Name = MonsterNameAdapter.From(hunt.Game, monster.Id),
            Icon = await MonsterIconAdapter.UriFrom(hunt.Game, monster.Id),
            TimeElapsed = hunt.FinishedAt - hunt.StartedAt,
            MaxHealth = monster.MaxHealth,
            HuntElapsed = huntElapsed,
            HealthSteps = healthSteps,
            Statuses = { BuildEnrage(hunt, huntElapsed ?? TimeSpan.Zero, monster.Enrage) },
            Players = hunt.Players.Select(it => PartyMemberDetailsViewModelBuilder.Build(hunt, monster, it))
                                  .FilterNull()
                                  .ToObservableCollection(),
            Crown = monster.Crown,
        };
    }

    [Obsolete]
    private static StatusDetailsViewModel BuildEnrage(
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
            Color = EnrageBrush,
            Name = Localization.QueryString("//Strings/Ailments/Rise/Ailment[@Id='STATUS_ENRAGE']"),
            UpTime = activationsTotalSeconds / Math.Max(1.0, timeElapsed),
            Activations = activations
        };
    }
}