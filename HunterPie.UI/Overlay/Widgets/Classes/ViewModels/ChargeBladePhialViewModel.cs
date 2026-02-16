using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class ChargeBladePhialViewModel : ViewModel
{
    public bool IsActive { get; set => SetValue(ref field, value); }
    public float Timer { get; set => SetValue(ref field, value); }
}