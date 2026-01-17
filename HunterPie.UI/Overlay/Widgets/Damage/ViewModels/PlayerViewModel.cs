using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class PlayerViewModel(DamageMeterWidgetConfig config) : ViewModel
{
    public DamageMeterWidgetConfig Config { get; } = config;

    public string Name
    {
        get;
        set => SetValue(ref field, value);
    }
    public Weapon Weapon
    {
        get;
        set => SetValue(ref field, value);
    }
    public int Damage
    {
        get;
        set => SetValue(ref field, value);
    }
    public double DPS
    {
        get;
        set => SetValue(ref field, value);
    }

    public DamageBarViewModel Bar { get; set => SetValue(ref field, value); }

    public bool IsIncreasing
    {
        get;
        set => SetValue(ref field, value);
    }
    public bool IsUser
    {
        get;
        set => SetValue(ref field, value);
    }

    public int MasterRank { get; set => SetValue(ref field, value); }

    public bool IsVisible { get; set => SetValue(ref field, value); }

    public double? Affinity { get; set => SetValue(ref field, value); } = null;

    public double? RawDamage { get; set => SetValue(ref field, value); } = null;

    public double? ElementalDamage { get; set => SetValue(ref field, value); } = null;
}