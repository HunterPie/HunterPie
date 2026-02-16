using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class LongSwordViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.Longsword;

    public float SpiritGaugeRegenTimer { get; set => SetValue(ref field, value); }
    public float SpiritGaugeRegenMaxTimer { get; set => SetValue(ref field, value); }
    public float SpiritGaugeBuildUp { get; set => SetValue(ref field, value); }
    public float SpiritGaugeMaxBuildUp { get; set => SetValue(ref field, value); }
    public int SpiritLevel { get; set => SetValue(ref field, value); }
    public float SpiritLevelTimer { get; set => SetValue(ref field, value); }
    public float SpiritLevelMaxTimer { get; set => SetValue(ref field, value); }
}