using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Brushes;
using HunterPie.UI.Architecture.Graphs;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Timers;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModel
{
    public class MockMeterViewModel : MeterViewModel
    {
        private int totalDamage = 0;
        private readonly Timer dispatcher;

        public MockMeterViewModel()
        {
            dispatcher = new(1000);
            dispatcher.Elapsed += MockInGameAction;
            dispatcher.Start();

            MockPlayers();
            MockPlayerSeries();
        }

        private void MockPlayers()
        {
            Players.Add(new()
            {
                Name = "Player 1",
                Weapon = Weapon.Bow,
                Color = "#c3baf4",
                Percentage = 25
            });
            Players.Add(new()
            {
                Name = "Player 2",
                Weapon = Weapon.ChargeBlade,
                Color = "#98ff98",
                Percentage = 25,
                IsUser = true
            });
            Players.Add(new()
            {
                Name = "Player 3",
                Color = "#FF4B8EEE",
                Weapon = Weapon.Greatsword,
                Percentage = 25
            });
            Players.Add(new()
            {
                Name = "Player 4",
                Color = "#FF10B9DE",
                Weapon = Weapon.HuntingHorn,
                Percentage = 25
            });
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


        private void MockPlayerSeries()
        {
            int i = 0;
            LinearSeriesCollectionBuilder builder = new();
            foreach (PlayerViewModel player in Players)
            {
                PlayerChartValues.Add(new());
                Color color = (Color)ColorConverter.ConvertFromString(player.Color);
                builder.AddSeries(PlayerChartValues[i], player.Name, color);
                i++;
            }
            Series = builder.Build();
        }
    }
}
