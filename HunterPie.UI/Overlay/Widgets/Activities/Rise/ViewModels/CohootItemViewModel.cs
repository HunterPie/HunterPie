using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class CohootItemViewModel : ViewModel
{
    public bool IsActive { get; set => SetValue(ref field, value); }
}