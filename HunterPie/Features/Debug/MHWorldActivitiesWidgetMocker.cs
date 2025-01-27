using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Activities.Views;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

namespace HunterPie.Features.Debug;

internal class MHWorldActivitiesWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockActivitiesWidget)
            return;

        var config = new ActivitiesWidgetConfig();

        var view = new ActivitiesView(config);

        MHWorldActivitiesViewModel activities = DependencyContainer.Get<MHWorldActivitiesViewModel>();

        SetupHarvestBox(activities.HarvestBox);
        SetupSteamworks(activities.Steamworks);

        view.ViewModel.Activities = activities;
        view.ViewModel.InVisibleStage = true;

        WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(
            widget: view
        );
    }

    private static void SetupHarvestBox(HarvestBoxViewModel viewModel)
    {
        viewModel.Count = 20;
        viewModel.MaxCount = 50;
        viewModel.Fertilizers.Add(
            item: new HarvestFertilizerViewModel
            {
                Fertilizer = Fertilizer.FungiL,
                IsExpiring = true,
                IsFirst = true,
            }.SetDays(3)
        );
        viewModel.Fertilizers.Add(
            item: new HarvestFertilizerViewModel
            {
                Fertilizer = Fertilizer.GrowthL,
                IsExpiring = false,
            }.SetDays(8)
        );
        viewModel.Fertilizers.Add(
            item: new HarvestFertilizerViewModel
            {
                Fertilizer = Fertilizer.HoneyL,
                IsExpiring = false,
            }.SetDays(9)
        );
        viewModel.Fertilizers.Add(
            item: new HarvestFertilizerViewModel
            {
                Fertilizer = Fertilizer.PlantL,
                IsExpiring = false,
            }.SetDays(7)
        );
    }

    private static void SetupSteamworks(SteamworksViewModel viewModel)
    {
        viewModel.MaxNaturalFuel = 700;
        viewModel.NaturalFuel = 584;
        viewModel.StoredFuel = 12_302;
    }
}