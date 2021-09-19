using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    /// <summary>
    /// Interaction logic for MonsterContainer.xaml
    /// </summary>
    public partial class MonsterContainer : UserControl, IWidget, IView<MonsterViewModel>
    {

        public MonsterContainer()
        {
            DataContext = new MonsterViewModel()
            {
                Health = 25000.0,
                MaxHealth = 25000.0,
                MaxStamina = 10000.0,
                Stamina = 10000.0,
                HealthPercentage = 100.00,
                Em = "em116",
                Name = "Dodogama",
                IsEnraged = false

            };
            InitializeComponent();
            
        }

        public IWidgetSettings Settings => ClientConfig.Config.Overlay.EndemicWidget;

        public string Title => "Monster Widget";

        public MonsterViewModel Model => (MonsterViewModel)DataContext;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window owner = Window.GetWindow(this);
            owner.PreviewKeyDown += (_, args) =>
            {
                switch (args.Key)
                {
                    case Key.Up:
                        Model.Health += 200.0;
                        Model.HealthPercentage = Model.Health / Model.MaxHealth * 100;
                        break;
                    case Key.Down:
                        Model.Health -= 200.0;
                        Model.HealthPercentage = Model.Health / Model.MaxHealth * 100;
                        break;
                    case Key.Left:
                        Model.Stamina -= 10.5;
                        break;
                    case Key.Right:
                        Model.Stamina += 21.5;
                        break;
                    case Key.E:
                        Model.IsEnraged = !Model.IsEnraged;
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
