using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Activities.View;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

namespace HunterPie.Features.Debug
{
    internal class ActivitiesWidgetMocker : IWidgetMocker
    {
        public void Mock()
        {
            if (ClientConfig.Config.Debug.MockActivitiesWidget)
                WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(new ActivitiesView()
                {
                    DataContext = new MockActivitiesViewModel()
                }
            );
        }
    }
}
