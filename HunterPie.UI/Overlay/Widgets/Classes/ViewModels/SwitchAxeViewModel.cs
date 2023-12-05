using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

public class SwitchAxeViewModel : ViewModel, IClassViewModel
{
    public Weapon WeaponId => Weapon.SwitchAxe;

    private float _powerBuffTimer;
    public float PowerBuffTimer { get => _powerBuffTimer; set => SetValue(ref _powerBuffTimer, value); }

    private float _maxPowerBuffTimer;
    public float MaxPowerBuffTimer { get => _maxPowerBuffTimer; set => SetValue(ref _maxPowerBuffTimer, value); }

    private float _buildUp;
    public float BuildUp { get => _buildUp; set => SetValue(ref _buildUp, value); }

    private float _maxBuildUp;
    public float MaxBuildUp { get => _maxBuildUp; set => SetValue(ref _maxBuildUp, value); }

    private float _chargedTimer;
    public float ChargedTimer { get => _chargedTimer; set => SetValue(ref _chargedTimer, value); }

    private float _maxChargeTimer;
    public float MaxChargeTimer { get => _maxChargeTimer; set => SetValue(ref _maxChargeTimer, value); }
}