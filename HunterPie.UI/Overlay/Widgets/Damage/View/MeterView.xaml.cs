using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModel;
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

namespace HunterPie.UI.Overlay.Widgets.Damage.View
{
    /// <summary>
    /// Interaction logic for MeterView.xaml
    /// </summary>
    public partial class MeterView : View<MeterViewModel>, IWidget<DamageMeterWidgetConfig>
    {
        public MeterView()
        {
            InitializeComponent();
        }

        public DamageMeterWidgetConfig Settings => ClientConfig.Config.Overlay.DamageMeterWidget;

        public string Title => "Damage Meter";
    }
}
