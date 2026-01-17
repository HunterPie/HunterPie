using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class ChargeBladeViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.ChargeBlade;

    public ObservableCollection<ChargeBladePhialViewModel> Phials { get; } = new();
    public PhialChargeLevel Charge { get; set => SetValue(ref field, value); }
    public float SwordBuff { get; set => SetValue(ref field, value); }
    public float AxeBuff { get; set => SetValue(ref field, value); }
    public float ShieldBuff { get; set => SetValue(ref field, value); }
    public float ChargeBuildUp { get; set => SetValue(ref field, value); }
    public float ChargeMaxBuildUp { get; set => SetValue(ref field, value); }
}