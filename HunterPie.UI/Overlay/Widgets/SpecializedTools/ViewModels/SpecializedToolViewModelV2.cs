using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;

public class SpecializedToolViewModelV2 : ViewModel
{
    private SpecializedToolType _id;
    public SpecializedToolType Id { get => _id; set => SetValue(ref _id, value); }

    private double _timer;
    public double Timer { get => _timer; set => SetValue(ref _timer, value); }

    private double _maxTimer;
    public double MaxTimer { get => _maxTimer; set => SetValue(ref _maxTimer, value); }

    private double _cooldown;
    public double Cooldown { get => _cooldown; set => SetValue(ref _cooldown, value); }

    private double _maxCooldown;
    public double MaxCooldown { get => _maxCooldown; set => SetValue(ref _maxCooldown, value); }

    private bool _isRecharging;
    public bool IsRecharging { get => _isRecharging; set => SetValue(ref _isRecharging, value); }

    private bool _isVisible;
    public bool IsVisible { get => _isVisible; set => SetValue(ref _isVisible, value); }
}