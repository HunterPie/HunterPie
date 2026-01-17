using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class FertilizerDayViewModel : ViewModel
{
    public bool IsActive { get; set => SetValue(ref field, value); }
}