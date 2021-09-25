using HunterPie.Core.Client;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System.Windows;
using System.Windows.Input;

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
