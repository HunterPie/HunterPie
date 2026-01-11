using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class SteamworksViewModel : ViewModel
{
    public int NaturalFuel { get; set => SetValue(ref field, value); }
    public int MaxNaturalFuel { get; set => SetValue(ref field, value); }
    public int StoredFuel { get; set => SetValue(ref field, value); }
}