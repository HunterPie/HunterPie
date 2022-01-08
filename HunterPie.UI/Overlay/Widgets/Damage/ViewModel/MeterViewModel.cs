using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModel
{
    public class MeterViewModel : Bindable
    {
        private static DamageMeterWidgetConfig Settings => ClientConfig.Config.Overlay.DamageMeterWidget;
        private double _timeElapsed = 1;
        private int _deaths;
        private int totalDamage = 0;
        private readonly Timer dispatcher;

        public ChartValues<ObservablePoint>[] PlayerChartValues { get; } = new ChartValues<ObservablePoint>[4]
        {
            new ChartValues<ObservablePoint>(),
            new ChartValues<ObservablePoint>(),
            new ChartValues<ObservablePoint>(),
            new ChartValues<ObservablePoint>(),
        };
        public SeriesCollection Series { get; private set; } = new();

        public ObservableCollection<PlayerViewModel> Players { get; } = new()
        {
            new()
            {
                Name = "Player 1",
                Weapon = Weapon.Bow,
                Color = "#c3baf4",
                Percentage = 25
            },
            new()
            {
                Name = "Player 2",
                Weapon = Weapon.ChargeBlade,
                Color = "#98ff98",
                Percentage = 25,
                IsUser = true
            },
            new()
            {
                Name = "Player 3",
                Color = "#FF4B8EEE",
                Weapon = Weapon.Greatsword,
                Percentage = 25
            },
            new()
            {
                Name = "Player 4",
                Color = "#FF10B9DE",
                Weapon = Weapon.HuntingHorn,
                Percentage = 25
            },
        };

        public double TimeElapsed
        {
            get => _timeElapsed;
            set { SetValue(ref _timeElapsed, value); }
        }

        public int Deaths
        {
            get => _deaths;
            set { SetValue(ref _deaths, value); }
        }

        public MeterViewModel()
        {
            dispatcher = new(1000);
            dispatcher.Elapsed += MockInGameAction;
            dispatcher.Start();
            MockCreatePlayerSeries();
        }

        private void MockInGameAction(object sender, EventArgs e)
        {
            Random random = new();
            int i = 1;
            foreach (PlayerViewModel player in Players)
            {
                double lastDps = player.DPS;
                int hit = random.Next(0, 400 / i);
                player.Damage += hit;
                player.DPS = player.Damage / TimeElapsed;
                player.Percentage = player.Damage / (double)Math.Max(1, totalDamage) * 100;
                player.IsIncreasing = lastDps < player.DPS;

                if (PlayerChartValues[i - 1].Count > 50)
                    PlayerChartValues[i - 1].RemoveAt(0);

                PlayerChartValues[i - 1].Add(new ObservablePoint(TimeElapsed, player.DPS));
                totalDamage += hit;
                i++;
            }
            TimeElapsed++;
        }

        public void ToggleHighlight() => Settings.ShouldHighlightMyself.Value = !Settings.ShouldHighlightMyself;
        public void ToggleBlur() => Settings.ShouldBlurNames.Value = !Settings.ShouldBlurNames;

        private void MockCreatePlayerSeries()
        {
            int i = 0;
            foreach (PlayerViewModel player in Players)
            {
                Color c = (Color)ColorConverter.ConvertFromString(player.Color);
                SolidColorBrush color = new SolidColorBrush(c);
                LinearGradientBrush fill = new LinearGradientBrush()
                {
                    StartPoint = new Point(1, 0),
                    EndPoint = new Point(1, 1)
                };
                fill.GradientStops = new GradientStopCollection()
                {
                    new GradientStop(c - Color.FromArgb(0xC0, 0, 0, 0), 0),
                    new GradientStop(c - Color.FromArgb(0xFF, 0, 0, 0), 0.6)
                };
                var newSeries = new LineSeries()
                {
                    Title = player.Name,
                    Stroke = color,
                    Fill = fill,
                    PointGeometrySize = 0,
                    StrokeThickness = 2,
                    LineSmoothness = 0.5
                };
                newSeries.Values = PlayerChartValues[i];
                Series.Add(newSeries);
                i++;
            }
        }
    }
}
