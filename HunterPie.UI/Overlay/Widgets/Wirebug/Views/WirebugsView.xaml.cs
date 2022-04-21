using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;
using System;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.Views
{
    /// <summary>
    /// Interaction logic for WirebugsView.xaml
    /// </summary>
    public partial class WirebugsView : View<WirebugsViewModel>, IWidget<WirebugWidgetConfig>, IWidgetWindow
    {
        private readonly WirebugWidgetConfig _config;
        
        public WirebugsView(WirebugWidgetConfig config)
        {
            _config = config;
            InitializeComponent();
        }

        public WirebugWidgetConfig Settings => _config;
        public string Title => "Wirebug Widget";
        public WidgetType Type => WidgetType.ClickThrough;
        IWidgetSettings IWidgetWindow.Settings => Settings;

        public event EventHandler<WidgetType> OnWidgetTypeChange;
    }
}
