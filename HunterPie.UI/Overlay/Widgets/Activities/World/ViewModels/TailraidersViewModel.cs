using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class TailraidersViewModel : ViewModel
{
    public int QuestsLeft { get; set => SetValue(ref field, value); }
    public bool IsDeployed { get; set => SetValue(ref field, value); }
}