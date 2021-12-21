using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Metrics.View
{
    /// <summary>
    /// Interaction logic for TelemetricsView.xaml
    /// </summary>
    public partial class TelemetricsView : View<TelemetricsViewModel>, IWidget<TelemetricsWidgetConfig>
    {
        public TelemetricsView()
        {
            InitializeComponent();
        }

        public TelemetricsWidgetConfig Settings => ClientConfig.Config.Overlay.DebugWidget;

        public string Title => "Debug Metrics";
    }
}
