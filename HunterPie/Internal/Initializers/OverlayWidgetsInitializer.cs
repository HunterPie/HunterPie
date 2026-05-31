using HunterPie.Domain.Interfaces;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Widgets.Abnormality.View;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Views;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using HunterPie.UI.Overlay.Widgets.Chat.Views;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;
using HunterPie.UI.Overlay.Widgets.Clock.ViewModels;
using HunterPie.UI.Overlay.Widgets.Clock.Views;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using HunterPie.UI.Overlay.Widgets.Metrics.View;
using HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using HunterPie.UI.Overlay.Widgets.Player.Views;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.Views;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModels;
using HunterPie.UI.Overlay.Widgets.Wirebug.Views;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class OverlayWidgetsInitializer(
    IWidgetProvider widgetProvider
) : IInitializer
{
    public Task Init()
    {
        widgetProvider.Bind<AbnormalityBarViewModel, AbnormalityBarView>();
        widgetProvider.Bind<ActivitiesViewModel, ActivitiesView>();
        widgetProvider.Bind<ChatViewModel, ChatView>();
        widgetProvider.Bind<ClassViewModel, ClassView>();
        widgetProvider.Bind<ClockViewModel, ClockView>();
        widgetProvider.Bind<MeterViewModelV2, MeterViewV2>();
        widgetProvider.Bind<TelemetricsViewModel, TelemetricsView>();
        widgetProvider.Bind<MonstersViewModel, MonstersView>();
        widgetProvider.Bind<PlayerHudViewModel, PlayerHudView>();
        widgetProvider.Bind<SpecializedToolViewModelV2, SpecializedToolViewV2>();
        widgetProvider.Bind<WirebugsViewModel, WirebugsView>();


        return Task.CompletedTask;
    }
}