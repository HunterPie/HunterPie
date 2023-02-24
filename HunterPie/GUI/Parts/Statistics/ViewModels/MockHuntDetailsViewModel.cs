using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Brushes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
public class MockHuntDetailsViewModel : HuntDetailsViewModel
{

    private readonly Color[] _colors = {
        (Color)ColorConverter.ConvertFromString("#f076a1"),
        (Color)ColorConverter.ConvertFromString("#f0cf76"),
        (Color)ColorConverter.ConvertFromString("#76f092"),
        (Color)ColorConverter.ConvertFromString("#76f0d8"),
    };

    private readonly int[] seeds = { 14580223, 3842582, 14252523, 1765317 };

    [Obsolete]
    public MockHuntDetailsViewModel()
    {
        MockMonster();
        MockHuntDetails();
        MockPlayers();
        MockAbnormalities();
    }

    private void MockHuntDetails()
    {
        TimeElapsed = 250;
        MonsterId = 0;
        HuntedAt = DateTime.Now;
    }

    private void MockMonster()
    {
        Sections.Add(new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection() { 4, 4 },
            Value = 0,
            SectionWidth = 58
        });

        Sections.Add(new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection() { 4, 4 },
            Value = 80,
            SectionWidth = 35
        });

        Sections.Add(new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection() { 4, 4 },
            Value = 153,
            SectionWidth = 53
        });

        Sections.Add(new AxisSection
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

    private void MockAbnormalities()
    {
        var rng = new Random(123);
        string[] abnormalities =
        {
            "ICON_DEFUP",
            "ICON_WINDRES+",
            "ICON_ENVNEG",
            "ICON_YELLOWSQUID",
            "ICON_POWERDRUM",
            "ELEMENT_FIRE",
            "ICON_BLEED",
            "ICON_DANGOGLUTTON"
        };
        Color[] colors =
        {
            Colors.GreenYellow,
            Colors.Gold,
            Colors.Yellow,
            Colors.Cyan,
            Colors.Violet,
            Colors.Turquoise,
            Colors.Teal
        };
        PartyMemberDetailsViewModel firstMember = Players.First();
        List<AbnormalityDetailsViewModel> vms = new(abnormalities.Length);
        foreach (string abnormality in abnormalities)
        {
            Brush color = new SolidColorBrush(colors[rng.Next(0, colors.Length)]);
            var sections = new List<AxisSection>();

            int nPoints = rng.Next(1, 4);
            double start = 0;
            Brush colorClone = color.Clone();
            colorClone.Opacity = 0.05;
            for (int i = 0; i < nPoints; i++)
            {
                start += rng.NextDouble() * 150;
                sections.Add(new AxisSection
                {
                    StrokeThickness = 0,
                    Fill = colorClone,
                    Value = start,
                    SectionWidth = rng.NextDouble() * 80
                });
            }

            var summary = new AbnormalityDetailsViewModel
            {
                Id = abnormality,
                Uptime = rng.NextDouble(),
                Color = color,
            };

            summary.Sections.AddRange(sections);

            vms.Add(summary);
        }

        foreach (AbnormalityDetailsViewModel vm in vms.OrderByDescending(vm => vm.Uptime).ToArray())
            firstMember.Abnormalities.Add(vm);
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
                new PartyMemberDetailsViewModel
                {
                    Name = playerName,
                    Series = damageSeries,
                    Weapon = (Weapon)rng.Next(0, 13),
                }
            );
        }

        SelectedPlayer = Players.First();
    }
}
