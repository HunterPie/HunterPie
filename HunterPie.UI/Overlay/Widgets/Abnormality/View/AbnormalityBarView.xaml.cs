using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
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
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.View
{
    /// <summary>
    /// Interaction logic for AbnormalityBarView.xaml
    /// </summary>
    public partial class AbnormalityBarView : View<AbnormalityBarViewModel>, IWidget<AbnormalityWidgetConfig>, IWidgetWindow
    {
        public AbnormalityBarView()
        {
            InitializeComponent();
        }

        public string Title => "Abnormality Widget";
        public AbnormalityWidgetConfig Settings => ClientConfig.Config.Overlay.AbnormalityWidget;
        public WidgetType Type => WidgetType.ClickThrough;
    }
}
