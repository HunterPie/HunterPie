using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class PlayerViewModel : ViewModel
{
    public DamageMeterWidgetConfig Config { get; }

    public PlayerViewModel(DamageMeterWidgetConfig config)
    {
        Config = config;
    }

    private string _name;
    public string Name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }

    private Weapon _weapon;
    public Weapon Weapon
    {
        get => _weapon;
        set => SetValue(ref _weapon, value);
    }

    private int _damage;
    public int Damage
    {
        get => _damage;
        set => SetValue(ref _damage, value);
    }

    private double _dps;
    public double DPS
    {
        get => _dps;
        set => SetValue(ref _dps, value);
    }

    private DamageBarViewModel _bar;
    public DamageBarViewModel Bar { get => _bar; set => SetValue(ref _bar, value); }

    private bool _isIncreasing;
    public bool IsIncreasing
    {
        get => _isIncreasing;
        set => SetValue(ref _isIncreasing, value);
    }

    private bool _isUser;
    public bool IsUser
    {
        get => _isUser;
        set => SetValue(ref _isUser, value);
    }

    private int _masterRank;
    public int MasterRank { get => _masterRank; set => SetValue(ref _masterRank, value); }

    private bool _isVisible;
    public bool IsVisible { get => _isVisible; set => SetValue(ref _isVisible, value); }

    private double? _affinity = null;
    public double? Affinity { get => _affinity; set => SetValue(ref _affinity, value); }

    private double? _rawDamage = null;
    public double? RawDamage { get => _rawDamage; set => SetValue(ref _rawDamage, value); }

    private double? _elementalDamage = null;
    public double? ElementalDamage { get => _elementalDamage; set => SetValue(ref _elementalDamage, value); }
}