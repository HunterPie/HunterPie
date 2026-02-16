using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class InsectGlaiveViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.InsectGlaive;

    public KinsectBuff PrimaryQueuedBuff { get; set => SetValue(ref field, value); }
    public KinsectBuff SecondaryQueuedBuff { get; set => SetValue(ref field, value); }
    public double Stamina { get; set => SetValue(ref field, value); }
    public double MaxStamina { get; set => SetValue(ref field, value); }
    public double AttackTimer { get; set => SetValue(ref field, value); }
    public double MovementSpeedTimer { get; set => SetValue(ref field, value); }
    public double DefenseTimer { get; set => SetValue(ref field, value); }
    public KinsectChargeType ChargeType { get; set => SetValue(ref field, value); }
    public double ChargeTimer { get; set => SetValue(ref field, value); }
}