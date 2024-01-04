﻿using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Skills;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;

public class MHWChargeBlade : MHWMeleeWeapon, IChargeBlade
{
    private readonly ISkillService _skillService;
    private float _shieldBuff;
    private float _swordBuff;
    private float _axeBuff;
    private float _chargeBuildUp;
    private PhialChargeLevel _charge;
    private int _phials;
    private int _maxPhials;

    public float ShieldBuff
    {
        get => _shieldBuff;
        private set
        {
            if (value == _shieldBuff)
                return;

            _shieldBuff = value;
            this.Dispatch(_onShieldBuffTimerChange, new ChargeBladeBuffTimerChangeEventArgs(value));
        }
    }

    public float SwordBuff
    {
        get => _swordBuff;
        private set
        {
            if (value == _swordBuff)
                return;

            _swordBuff = value;
            this.Dispatch(_onSwordBuffTimerChange, new ChargeBladeBuffTimerChangeEventArgs(value));
        }
    }

    public float AxeBuff
    {
        get => _axeBuff;
        private set
        {
            if (value == _axeBuff)
                return;

            _axeBuff = value;
            this.Dispatch(_onAxeBuffTimerChange, new ChargeBladeBuffTimerChangeEventArgs(value));
        }
    }

    public float ChargeBuildUp
    {
        get => _chargeBuildUp;
        private set
        {
            if (value == _chargeBuildUp)
                return;

            _chargeBuildUp = value;
            this.Dispatch(_onChargeBuildUpChange, new ChargeBladeBuildUpChangeEventArgs(value, MaxChargeBuildUp));
        }
    }

    public float MaxChargeBuildUp => 100.0f;

    public PhialChargeLevel Charge
    {
        get => _charge;
        private set
        {
            if (value == _charge)
                return;

            _charge = value;
            this.Dispatch(_onPhialsChange, new ChargeBladePhialChangeEventArgs(this));
        }
    }

    public int Phials
    {
        get => _phials;
        private set
        {
            if (value == _phials)
                return;

            _phials = value;
            this.Dispatch(_onPhialsChange, new ChargeBladePhialChangeEventArgs(this));
        }
    }

    public int MaxPhials
    {
        get => _maxPhials;
        private set
        {
            if (value == _maxPhials)
                return;

            _maxPhials = value;
            this.Dispatch(_onPhialsChange, new ChargeBladePhialChangeEventArgs(this));
        }
    }

    private readonly SmartEvent<ChargeBladeBuffTimerChangeEventArgs> _onShieldBuffTimerChange = new();
    public event EventHandler<ChargeBladeBuffTimerChangeEventArgs> OnShieldBuffTimerChange
    {
        add => _onShieldBuffTimerChange.Hook(value);
        remove => _onShieldBuffTimerChange.Unhook(value);
    }

    private readonly SmartEvent<ChargeBladeBuffTimerChangeEventArgs> _onSwordBuffTimerChange = new();
    public event EventHandler<ChargeBladeBuffTimerChangeEventArgs> OnSwordBuffTimerChange
    {
        add => _onSwordBuffTimerChange.Hook(value);
        remove => _onSwordBuffTimerChange.Unhook(value);
    }

    private readonly SmartEvent<ChargeBladeBuffTimerChangeEventArgs> _onAxeBuffTimerChange = new();
    public event EventHandler<ChargeBladeBuffTimerChangeEventArgs> OnAxeBuffTimerChange
    {
        add => _onAxeBuffTimerChange.Hook(value);
        remove => _onAxeBuffTimerChange.Unhook(value);
    }

    private readonly SmartEvent<ChargeBladeBuildUpChangeEventArgs> _onChargeBuildUpChange = new();
    public event EventHandler<ChargeBladeBuildUpChangeEventArgs> OnChargeBuildUpChange
    {
        add => _onChargeBuildUpChange.Hook(value);
        remove => _onChargeBuildUpChange.Unhook(value);
    }

    private readonly SmartEvent<ChargeBladePhialChangeEventArgs> _onPhialsChange = new();
    public event EventHandler<ChargeBladePhialChangeEventArgs> OnPhialsChange
    {
        add => _onPhialsChange.Hook(value);
        remove => _onPhialsChange.Unhook(value);
    }

    public MHWChargeBlade(
        IProcessManager process,
        ISkillService skillService
    ) : base(skillService, process, Weapon.ChargeBlade)
    {
        _skillService = skillService;
    }

    [ScannableMethod]
    private void GetData()
    {
        MHWChargeBladeStructure structure = Memory.Deref<MHWChargeBladeStructure>(
            AddressMap.GetAbsolute("WEAPON_MECHANICS_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_MECHANICS_OFFSETS")
        );

        Skill capacityBoost = _skillService.GetSkillBy(69);
        MaxPhials = 5 + (capacityBoost.Level > 0 ? 1 : 0);
        Phials = structure.Phials;
        Charge = structure.ChargeBuildUp.ToPhialChargeLevel();
        ChargeBuildUp = structure.ChargeBuildUp;

        // TODO: Multiply buffs by power prolonger buff
        ShieldBuff = structure.ShieldBuff;
        SwordBuff = structure.SwordBuff;
        AxeBuff = structure.AxeBuff;
    }

    public override void Dispose()
    {
        new IDisposable[]
        {
            _onShieldBuffTimerChange,
            _onSwordBuffTimerChange,
            _onAxeBuffTimerChange,
            _onChargeBuildUpChange,
            _onPhialsChange
        }.DisposeAll();

        base.Dispose();
    }
}