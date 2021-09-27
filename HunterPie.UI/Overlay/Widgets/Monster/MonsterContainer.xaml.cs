using HunterPie.Core.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    /// <summary>
    /// Interaction logic for MonsterContainer.xaml
    /// </summary>
    public partial class MonsterContainer : View<MonsterViewModel>, IWidget 
    {

        public MonsterContainer()
        {
            InitializeComponent();
            
        }

        public IWidgetSettings Settings => ClientConfig.Config.Overlay.EndemicWidget;

        public string Title => "Monster Widget";

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.MaxHealth = 25000.0;
            ViewModel.Health = 25000.0;
            ViewModel.MaxStamina = 10000.0;
            ViewModel.Stamina = 10000.0;
            ViewModel.HealthPercentage = 100.00;
            ViewModel.Em = "em116";
            ViewModel.Name = "Dodogama";
            ViewModel.IsEnraged = false;
            ViewModel.Crown = Crown.Gold;
            
            for (int i = 0; i < 13; i++)
            {
                ViewModel.Parts.Add(
                    new MonsterPartViewModel()
                    {
                        Name = $"Part {i}",
                        Health = 2000.0,
                        MaxHealth = 2000.0,
                        Tenderize = 300.0,
                        MaxTenderize = 300.0
                    }
                );
            }
            var updater = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            Random rng = new();
            updater.Tick += (_, __) =>
            {
                foreach (var abnorm in ViewModel.Parts)
                {
                    abnorm.Health -= rng.NextDouble() * 100;
                    abnorm.Tenderize--;

                    if (abnorm.Health <= 0.0)
                        abnorm.Health = abnorm.MaxHealth;

                    if (abnorm.Tenderize <= 0.0)
                        abnorm.Tenderize = abnorm.MaxTenderize;

                }

            };
            updater.Start();
            Window owner = Window.GetWindow(this);
            owner.PreviewKeyDown += (_, args) =>
            {
                switch (args.Key)
                {
                    case Key.Up:
                        ViewModel.Health += 200.0;
                        ViewModel.HealthPercentage = ViewModel.Health / ViewModel.MaxHealth * 100;
                        break;
                    case Key.Down:
                        ViewModel.Health -= 200.0;
                        ViewModel.HealthPercentage = ViewModel.Health / ViewModel.MaxHealth * 100;
                        break;
                    case Key.Left:
                        ViewModel.Stamina -= 10.5;
                        break;
                    case Key.Right:
                        ViewModel.Stamina += 21.5;
                        break;
                    case Key.E:
                        ViewModel.IsEnraged = !ViewModel.IsEnraged;
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
