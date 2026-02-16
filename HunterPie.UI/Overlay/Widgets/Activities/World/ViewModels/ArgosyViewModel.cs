using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class ArgosyViewModel : ViewModel
{
    public int DaysLeft { get; set => SetValue(ref field, value); }
    public bool IsInTown { get; set => SetValue(ref field, value); }
}