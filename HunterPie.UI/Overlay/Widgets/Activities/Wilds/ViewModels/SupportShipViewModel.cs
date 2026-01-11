using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class SupportShipViewModel : ViewModel
{
    public bool IsAvailable { get; set => SetValue(ref field, value); }
    public int Days { get; set => SetValue(ref field, value); }
}