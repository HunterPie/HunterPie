using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Activities;

internal class ActivitiesWidgetModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        // Monster Hunter World
        registry
            .WithFactory<HarvestBoxViewModel>()
            .WithFactory<TailraidersViewModel>()
            .WithFactory<SteamworksViewModel>()
            .WithFactory<ArgosyViewModel>()
            .WithFactory<MHWorldActivitiesViewModel>();

        // Monster Hunter Rise
        registry
            .WithFactory<CohootNestViewModel>()
            .WithFactory<CohootNestsViewModel>()
            .WithFactory<MeowcenariesViewModel>()
            .WithFactory<SubmarinesViewModel>()
            .WithFactory<TrainingDojoViewModel>()
            .WithFactory<MHRiseActivitiesViewModel>();

        // Monster Hunter Wilds
        registry
            .WithFactory<MaterialRetrievalViewModel>()
            .WithFactory<SupportShipViewModel>()
            .WithFactory<IngredientsCenterViewModel>()
            .WithFactory<MHWildsActivitiesViewModel>();
    }
}