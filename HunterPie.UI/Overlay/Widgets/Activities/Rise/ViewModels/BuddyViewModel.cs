using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class BuddyViewModel : Bindable
{
    public bool IsEmpty { get; set => SetValue(ref field, value); }
    public string Name { get; set => SetValue(ref field, value); }
    public int Level { get; set => SetValue(ref field, value); }
}