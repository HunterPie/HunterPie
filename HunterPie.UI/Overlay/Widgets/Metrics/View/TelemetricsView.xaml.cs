using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;
using System;

namespace HunterPie.UI.Overlay.Widgets.Metrics.View
{
    /// <summary>
    /// Interaction logic for TelemetricsView.xaml
    /// </summary>
    public partial class TelemetricsView : View<TelemetricsViewModel>, IWidget<TelemetricsWidgetConfig>, IWidgetWindow
    {
        public TelemetricsView()
        {
            InitializeComponent();
        }

        public TelemetricsWidgetConfig Settings => ClientConfig.Config.Overlay.DebugWidget;

        public string Title => "Debug Metrics";

        public WidgetType Type => WidgetType.ClickThrough;

        IWidgetSettings IWidgetWindow.Settings => Settings;

        private void OnGCClick(object sender, EventArgs e)
        {
            ViewModel.ExecuteGarbageCollector();
        }
    }
}
