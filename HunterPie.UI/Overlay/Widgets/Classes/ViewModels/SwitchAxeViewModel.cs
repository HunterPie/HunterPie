using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class SwitchAxeViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.SwitchAxe;

    public float PowerBuffTimer { get; set => SetValue(ref field, value); }
    public float MaxPowerBuffTimer { get; set => SetValue(ref field, value); }
    public float BuildUp { get; set => SetValue(ref field, value); }
    public float MaxBuildUp { get; set => SetValue(ref field, value); }
    public float LowBuildUp { get; set => SetValue(ref field, value); }
    public bool IsChargeActive { get; set => SetValue(ref field, value); }
    public float ChargedBuildUp { get; set => SetValue(ref field, value); }
    public float MaxChargedBuildUp { get; set => SetValue(ref field, value); }
    public float ChargedTimer { get; set => SetValue(ref field, value); }
    public float MaxChargedTimer { get; set => SetValue(ref field, value); }
}