using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModel;
using System;

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

        private void OnPlayerHighlightToggle(object sender, EventArgs e) => ViewModel.ToggleHighlight();
        private void OnPlayerBlurToggle(object sender, EventArgs e) => ViewModel.ToggleBlur();
    }
}
