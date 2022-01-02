using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModel
{
    internal class MockMeterViewModel : Bindable
    {
        private double secondsElapsed = 1;
        private int damageCap = int.MaxValue;
        private int totalDamage = 0;
        private readonly DispatcherTimer dispatcher;

        public ObservableCollection<PlayerViewModel> Players { get; } = new()
        {
            new()
            {
                Name = "Player 1",
                Weapon = Weapon.Bow,
                Color = "#ff9966",
                Percentage = 25
            },
            new()
            {
                Name = "Player 2",
                Weapon = Weapon.ChargeBlade,
                Color = "#ff5e62",
                Percentage = 25,
                IsUser = true
            },
            new()
            {
                Name = "Player 3",
                Color = "#d9a7c7",
                Weapon = Weapon.Greatsword,
                Percentage = 25
            },
            new()
            {
                Name = "Player 4",
                Color = "#fffcdc",
                Weapon = Weapon.HuntingHorn,
                Percentage = 25
            },
        };

        public double TimeElapsed
        {
            get => secondsElapsed;
            set
            {
                SetValue(ref secondsElapsed, value);
            }
        }

        public MockMeterViewModel()
        {
            dispatcher = new(DispatcherPriority.Render);
            dispatcher.Tick += MockInGameAction;
            dispatcher.Interval = new TimeSpan(0, 0, 10);
            dispatcher.Start();
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
                player.DPS = player.Damage / secondsElapsed;
                player.Percentage = player.Damage / (double)Math.Max(1, totalDamage) * 100;
                player.IsIncreasing = lastDps < player.DPS;

                totalDamage += hit;
                i++;
            }

            TimeElapsed++;
        }
    }
}
