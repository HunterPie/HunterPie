using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class SubmarineBoostViewModel : ViewModel
{
    public bool IsActive { get; set => SetValue(ref field, value); }
    public bool IsExtraBoost { get; set => SetValue(ref field, value); }
}