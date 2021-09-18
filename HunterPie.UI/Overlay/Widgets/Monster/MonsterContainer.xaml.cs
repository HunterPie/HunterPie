using HunterPie.Core.Client;
using HunterPie.Core.Settings;
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
    public partial class MonsterContainer : UserControl, IWidget
    {
        public MonsterContainer()
        {
            InitializeComponent();
        }

        public IWidgetSettings Settings => ClientConfig.Config.Overlay.EndemicWidget;

        public string Title => "Monster Widget";
    }
}
