using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Player.ViewModels;

public class WeaponSharpnessViewModel : ViewModel
{
    public Sharpness SharpnessLevel { get; set => SetValue(ref field, value); }
    public int Sharpness { get; set => SetValue(ref field, value); }
    public int MaxSharpness { get; set => SetValue(ref field, value); }
    public int HitsLeft { get; set => SetValue(ref field, value); }
}