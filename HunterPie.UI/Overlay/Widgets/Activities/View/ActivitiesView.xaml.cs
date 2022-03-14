using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.View
{
    /// <summary>
    /// Interaction logic for ActivitiesView.xaml
    /// </summary>
    public partial class ActivitiesView : View<ActivitiesViewModel>, IWidget<ActivitiesWidgetConfig>, IWidgetWindow
    {
        public ActivitiesView()
        {
            InitializeComponent();
        }

        public WidgetType Type => WidgetType.ClickThrough;

        public string Title => "Activities Widget";

        public ActivitiesWidgetConfig Settings => ClientConfig.Config.Overlay.ActivitiesWidget;
        IWidgetSettings IWidgetWindow.Settings => Settings;
    }
}
