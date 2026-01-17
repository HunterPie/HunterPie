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

internal class OverlayWidgetsInitializer(IWidgetProvider widgetProvider) : IInitializer
{
    private readonly IWidgetProvider _widgetProvider = widgetProvider;

    public Task Init()
    {
        _widgetProvider.Bind<AbnormalityBarViewModel, AbnormalityBarView>();
        _widgetProvider.Bind<ActivitiesViewModel, ActivitiesView>();
        _widgetProvider.Bind<ChatViewModel, ChatView>();
        _widgetProvider.Bind<ClassViewModel, ClassView>();
        _widgetProvider.Bind<ClockViewModel, ClockView>();
        _widgetProvider.Bind<MeterViewModelV2, MeterViewV2>();
        _widgetProvider.Bind<TelemetricsViewModel, TelemetricsView>();
        _widgetProvider.Bind<MonstersViewModel, MonstersView>();
        _widgetProvider.Bind<PlayerHudViewModel, PlayerHudView>();
        _widgetProvider.Bind<SpecializedToolViewModelV2, SpecializedToolViewV2>();
        _widgetProvider.Bind<WirebugsViewModel, WirebugsView>();


        return Task.CompletedTask;
    }
}