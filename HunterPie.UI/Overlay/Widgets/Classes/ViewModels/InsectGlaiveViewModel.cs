using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class InsectGlaiveViewModel : ViewModel
{
    private KinsectBuff _primaryQueuedBuff;
    public KinsectBuff PrimaryQueuedBuff { get => _primaryQueuedBuff; set => SetValue(ref _primaryQueuedBuff, value); }

    private KinsectBuff _secondaryQueuedBuff;
    public KinsectBuff SecondaryQueuedBuff { get => _secondaryQueuedBuff; set => SetValue(ref _secondaryQueuedBuff, value); }

    private double _stamina;
    public double Stamina { get => _stamina; set => SetValue(ref _stamina, value); }

    private double _attackTimer;
    public double AttackTimer { get => _attackTimer; set => SetValue(ref _attackTimer, value); }

    private double _movementSpeedTimer;
    public double MovementSpeedTimer { get => _movementSpeedTimer; set => SetValue(ref _movementSpeedTimer, value); }

    private double _defenseTimer;
    public double DefenseTimer { get => _defenseTimer; set => SetValue(ref _defenseTimer, value); }
}