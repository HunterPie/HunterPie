using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
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

namespace HunterPie.UI.Overlay.Widgets.Player.Views
{
    /// <summary>
    /// Interaction logic for PlayerHudView.xaml
    /// </summary>
    public partial class PlayerHudView : View<PlayerHudViewModel>
    {
        public PlayerHudView()
        {
            InitializeComponent();
        }
    }
}
