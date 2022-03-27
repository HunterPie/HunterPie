using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModel;
using System;

namespace HunterPie.UI.Overlay.Widgets.Damage.View
{
    /// <summary>
    /// Interaction logic for MeterView.xaml
    /// </summary>
    public partial class MeterView : View<MeterViewModel>, IWidget<DamageMeterWidgetConfig>, IWidgetWindow
    {
        public MeterView()
        {
            InitializeComponent();
        }

        public DamageMeterWidgetConfig Settings => ClientConfig.Config.Overlay.DamageMeterWidget;

        public string Title => "Damage Meter";

        public WidgetType Type => WidgetType.ClickThrough;

        IWidgetSettings IWidgetWindow.Settings => Settings;

        public event EventHandler<WidgetType> OnWidgetTypeChange;

        private void OnPlayerHighlightToggle(object sender, EventArgs e) => ViewModel.ToggleHighlight();
        private void OnPlayerBlurToggle(object sender, EventArgs e) => ViewModel.ToggleBlur();
    }
}
