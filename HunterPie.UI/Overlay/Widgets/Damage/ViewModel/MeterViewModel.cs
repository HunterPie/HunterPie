using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModel
{
    public class MeterViewModel : Bindable
    {
        private static DamageMeterWidgetConfig Settings => ClientConfig.Config.Overlay.DamageMeterWidget;
        private double _timeElapsed = 1;
        private int _deaths;
        private int totalDamage = 0;
        private readonly DispatcherTimer dispatcher;

        public ObservableCollection<PlayerViewModel> Players { get; } = new()
        {
            new()
            {
                Name = "Sciss",
                Weapon = Weapon.Bow,
                Color = "#ff9966",
                Percentage = 25
            },
            new()
            {
                Name = "Haato",
                Weapon = Weapon.ChargeBlade,
                Color = "#ff5e62",
                Percentage = 25,
                IsUser = true
            },
            new()
            {
                Name = "UwU",
                Color = "#d9a7c7",
                Weapon = Weapon.Greatsword,
                Percentage = 25
            },
            new()
            {
                Name = "HunterPie v2",
                Color = "#fffcdc",
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
            dispatcher = new(DispatcherPriority.Render);
            dispatcher.Tick += MockInGameAction;
            dispatcher.Interval = new TimeSpan(0, 0, 1);
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
                player.DPS = player.Damage / TimeElapsed;
                player.Percentage = player.Damage / (double)Math.Max(1, totalDamage) * 100;
                player.IsIncreasing = lastDps < player.DPS;

                totalDamage += hit;
                i++;
            }

            TimeElapsed++;
        }

        public void ToggleHighlight() => Settings.ShouldHighlightMyself.Value = !Settings.ShouldHighlightMyself;
        public void ToggleBlur() => Settings.ShouldBlurNames.Value = !Settings.ShouldBlurNames;
    }
}
