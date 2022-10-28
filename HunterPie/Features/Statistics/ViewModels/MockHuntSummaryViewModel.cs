using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Brushes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.ViewModels;
public class MockHuntSummaryViewModel : HuntSummaryViewModel
{

    private readonly Color[] _colors = {
        (Color)ColorConverter.ConvertFromString("#f076a1"),
        (Color)ColorConverter.ConvertFromString("#f0cf76"),
        (Color)ColorConverter.ConvertFromString("#76f092"),
        (Color)ColorConverter.ConvertFromString("#76f0d8"),
    };

    private readonly int[] seeds = { 14580223, 3842582, 14252523, 1765317 };

    [Obsolete]
    public MockHuntSummaryViewModel()
    {
        MockMonster();
        MockHuntDetails();
        MockPlayers();
    }

    private void MockHuntDetails()
    {
        TimeElapsed = 250;
        MonsterId = 0;
        HuntedAt = DateTime.Now;
    }

    private void MockMonster()
    {
        EnrageSections.Add(new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection() { 4, 4 },
            Value = 0,
            SectionWidth = 58
        });

        EnrageSections.Add(new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection() { 4, 4 },
            Value = 80,
            SectionWidth = 35
        });

        EnrageSections.Add(new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection() { 4, 4 },
            Value = 153,
            SectionWidth = 53
        });

        EnrageSections.Add(new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection() { 4, 4 },
            Value = 235,
            SectionWidth = 15
        });
    }

    private ChartValues<ObservablePoint> MockPoints(int index)
    {
        ChartValues<ObservablePoint> points = new();
        var rng = new Random(seeds[index]);
        double totalDamage = 0;
        for (float timeElapsed = 1; timeElapsed <= TimeElapsed; timeElapsed += 1f)
        {
            if (rng.NextDouble() <= rng.NextDouble())
                continue;

            totalDamage += rng.NextDouble() * rng.Next(0, 150 * (index + 1));

            points.Add(new ObservablePoint
            {
                X = timeElapsed,
                Y = totalDamage / timeElapsed
            });
        }

        return points;
    }

    private void MockPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            var rng = new Random();
            string playerName = $"Player {i + 1}";
            Series damageSeries = new LineSeries
            {
                Title = playerName,
                Stroke = new SolidColorBrush(_colors[i]),
                Fill = ColorFadeGradient.FromColor(_colors[i]),
                PointGeometrySize = 1,
                StrokeThickness = 2,
                LineSmoothness = 0,
                Values = MockPoints(i)
            };

            DamageSeries.Add(damageSeries);
            Players.Add(
                new PartyMemberSummaryViewModel
                {
                    Name = playerName,
                    Series = damageSeries,
                    Weapon = (Weapon)rng.Next(0, 13)
                }
            );
        }
    }
}
