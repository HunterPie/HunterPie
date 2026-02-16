using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class DualBladesViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.DualBlades;

    public bool InDemonMode { get; set => SetValue(ref field, value); }
    public float DemonBuildUp { get; set => SetValue(ref field, value); }
    public float DemonMaxBuildUp { get; set => SetValue(ref field, value); }
    public float PiercingBindTimer { get; set => SetValue(ref field, value); }
    public float PiercingBindMaxTimer { get; set => SetValue(ref field, value); }
    public bool InArchDemonMode { get; set => SetValue(ref field, value); }
    public bool IsPiercingBindVisible { get; set => SetValue(ref field, value); }
}