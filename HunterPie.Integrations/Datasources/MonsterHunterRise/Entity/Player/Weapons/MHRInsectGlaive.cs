using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;
public class MHRInsectGlaive : MHRMeleeWeapon, IInsectGlaive
{
    private KinsectBuff _primaryExtract;
    private KinsectBuff _secondaryExtract;
    private float _attackTimer;
    private float _speedTimer;
    private float _defenseTimer;

    public KinsectBuff PrimaryExtract
    {
        get => _primaryExtract;
        private set
        {
            if (value == _primaryExtract)
                return;

            _primaryExtract = value;
            this.Dispatch(_onPrimaryExtractChange, new InsectGlaiveExtractChangeEventArgs(value));
        }
    }

    public KinsectBuff SecondaryExtract
    {
        get => _secondaryExtract;
        private set
        {
            if (value == _secondaryExtract)
                return;

            _secondaryExtract = value;
            this.Dispatch(_onSecondaryExtractChange, new InsectGlaiveExtractChangeEventArgs(value));
        }
    }

    public float AttackTimer
    {
        get => _attackTimer;
        private set
        {
            if (value == _attackTimer)
                return;

            _attackTimer = value;
            this.Dispatch(_onAttackTimerChange, new InsectGlaiveBuffTimerChangeEventArgs(value));
        }
    }

    public float SpeedTimer
    {
        get => _speedTimer;
        private set
        {
            if (value == _speedTimer)
                return;

            _speedTimer = value;
            this.Dispatch(_onSpeedTimerChange, new InsectGlaiveBuffTimerChangeEventArgs(value));
        }
    }

    public float DefenseTimer
    {
        get => _defenseTimer;
        private set
        {
            if (value == _defenseTimer)
                return;

            _defenseTimer = value;
            this.Dispatch(_onDefenseTimerChange, new InsectGlaiveBuffTimerChangeEventArgs(value));
        }
    }

    private readonly SmartEvent<InsectGlaiveExtractChangeEventArgs> _onPrimaryExtractChange = new();
    public event EventHandler<InsectGlaiveExtractChangeEventArgs> OnPrimaryExtractChange
    {
        add => _onPrimaryExtractChange.Hook(value);
        remove => _onPrimaryExtractChange.Unhook(value);
    }

    private readonly SmartEvent<InsectGlaiveExtractChangeEventArgs> _onSecondaryExtractChange = new();
    public event EventHandler<InsectGlaiveExtractChangeEventArgs> OnSecondaryExtractChange
    {
        add => _onSecondaryExtractChange.Hook(value);
        remove => _onSecondaryExtractChange.Unhook(value);
    }

    private readonly SmartEvent<InsectGlaiveBuffTimerChangeEventArgs> _onAttackTimerChange = new();
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnAttackTimerChange
    {
        add => _onAttackTimerChange.Hook(value);
        remove => _onAttackTimerChange.Unhook(value);
    }

    private readonly SmartEvent<InsectGlaiveBuffTimerChangeEventArgs> _onSpeedTimerChange = new();
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnSpeedTimerChange
    {
        add => _onSpeedTimerChange.Hook(value);
        remove => _onSpeedTimerChange.Unhook(value);
    }

    private readonly SmartEvent<InsectGlaiveBuffTimerChangeEventArgs> _onDefenseTimerChange = new();
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnDefenseTimerChange
    {
        add => _onDefenseTimerChange.Hook(value);
        remove => _onDefenseTimerChange.Unhook(value);
    }

    public MHRInsectGlaive(IProcessManager process, Weapon id) : base(process, id) { }

    [ScannableMethod]
    private void GetKinsectExtracts()
    {

    }
}
