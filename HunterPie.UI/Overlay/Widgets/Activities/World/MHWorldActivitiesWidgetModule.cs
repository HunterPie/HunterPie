using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Activities.World;

internal class MHWorldActivitiesWidgetModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<HarvestBoxViewModel>()
            .WithService<TailraidersViewModel>()
            .WithService<SteamworksViewModel>()
            .WithService<ArgosyViewModel>()
            .WithService<MHWorldActivitiesViewModel>();
    }
}