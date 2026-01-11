using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class MHWorldActivitiesViewModel(
    HarvestBoxViewModel harvestBox,
    TailraidersViewModel tailraiders,
    SteamworksViewModel steamworks,
    ArgosyViewModel argosy) : ViewModel, IActivitiesViewModel
{
    public Observable<bool> IsHarvestBoxEnabled { get; } =
        ClientConfig.Config.World.Overlay.ActivitiesWidget.IsHarvestBoxEnabled;

    public Observable<bool> IsArgosyEnabled { get; } =
        ClientConfig.Config.World.Overlay.ActivitiesWidget.IsArgosyEnabled;

    public Observable<bool> IsMeowmastersEnabled { get; } =
        ClientConfig.Config.World.Overlay.ActivitiesWidget.IsMeowmastersEnabled;

    public Observable<bool> IsSteamworksEnabled { get; } =
        ClientConfig.Config.World.Overlay.ActivitiesWidget.IsSteamworksEnabled;


    public HarvestBoxViewModel HarvestBox { get; } = harvestBox;
    public TailraidersViewModel Tailraiders { get; } = tailraiders;
    public SteamworksViewModel Steamworks { get; } = steamworks;
    public ArgosyViewModel Argosy { get; } = argosy;
}