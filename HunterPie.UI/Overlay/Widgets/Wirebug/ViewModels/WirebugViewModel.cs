using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModels;

public class WirebugViewModel : ViewModel
{

    private double _cooldown;
    public double Cooldown { get => _cooldown; set => SetValue(ref _cooldown, value); }

    private double _maxCooldown;
    public double MaxCooldown { get => _maxCooldown; set => SetValue(ref _maxCooldown, value); }

    private double _timer;
    public double Timer { get => _timer; set => SetValue(ref _timer, value); }

    private double _maxTimer;
    public double MaxTimer { get => _maxTimer; set => SetValue(ref _maxTimer, value); }

    private bool _onCooldown;
    public bool OnCooldown { get => _onCooldown; set => SetValue(ref _onCooldown, value); }

    private bool _isAvailable;
    public bool IsAvailable { get => _isAvailable; set => SetValue(ref _isAvailable, value); }

    private bool _isTemporary;
    public bool IsTemporary { get => _isTemporary; set => SetValue(ref _isTemporary, value); }

    private WirebugState _wirebugState;
    public WirebugState WirebugState { get => _wirebugState; set => SetValue(ref _wirebugState, value); }
}