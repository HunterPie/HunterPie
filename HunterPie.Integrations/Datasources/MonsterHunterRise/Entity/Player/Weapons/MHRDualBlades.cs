using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;
using HunterPie.Core.Utils;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;

public sealed class MHRDualBlades(
    IGameProcess process,
    IScanService scanService) : MHRMeleeWeapon(process, scanService, Weapon.DualBlades), IDualBlades
{
    public bool IsDemonMode
    {
        get;
        private set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(_onDemonModeStateChange, new StateChangeEventArgs<bool>(value, !value));
        }
    }

    public bool IsArchDemonMode
    {
        get;
        private set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(_onArchDemonModeStateChange, new StateChangeEventArgs<bool>(value, !value));
        }
    }

    public float DemonBuildUp
    {
        get;
        private set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(_onDemonBuildUpChange, new BuildUpChangeEventArgs(value, MaxDemonBuildUp));
        }
    }

    public float MaxDemonBuildUp { get; private set; }

    public float PiercingBindTimer
    {
        get;
        private set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(_onPiercingBindTimerChange, new TimerChangeEventArgs(value, MaxPiercingBindTimer));
        }
    }

    public float MaxPiercingBindTimer { get; private set; }

    private readonly SmartEvent<StateChangeEventArgs<bool>> _onDemonModeStateChange = new();
    public event EventHandler<StateChangeEventArgs<bool>> OnDemonModeStateChange
    {
        add => _onDemonModeStateChange.Hook(value);
        remove => _onDemonModeStateChange.Unhook(value);
    }

    private readonly SmartEvent<StateChangeEventArgs<bool>> _onArchDemonModeStateChange = new();
    public event EventHandler<StateChangeEventArgs<bool>> OnArchDemonModeStateChange
    {
        add => _onArchDemonModeStateChange.Hook(value);
        remove => _onArchDemonModeStateChange.Unhook(value);
    }

    private readonly SmartEvent<BuildUpChangeEventArgs> _onDemonBuildUpChange = new();
    public event EventHandler<BuildUpChangeEventArgs> OnDemonBuildUpChange
    {
        add => _onDemonBuildUpChange.Hook(value);
        remove => _onDemonBuildUpChange.Unhook(value);
    }

    private readonly SmartEvent<TimerChangeEventArgs> _onPiercingBindTimerChange = new();
    public event EventHandler<TimerChangeEventArgs> OnPiercingBindTimerChange
    {
        add => _onPiercingBindTimerChange.Hook(value);
        remove => _onPiercingBindTimerChange.Unhook(value);
    }

    [ScannableMethod]
    private async Task GetData()
    {
        MHRDualBladesStructure structure = await Memory.DerefAsync<MHRDualBladesStructure>(
            address: AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("CURRENT_WEAPON_OFFSETS")
        );

        IsDemonMode = structure.IsDemonModeActive;
        IsArchDemonMode = structure.IsArchDemonModeActive;
        MaxDemonBuildUp = structure.DemonBuildUpMax;
        DemonBuildUp = structure.DemonBuildUp;

        await GetPiercingBind(structure.PiercingBindArrayPointer);
    }

    private async Task GetPiercingBind(nint arrayPointer)
    {
        nint[] piercingBindPtrs = (await Memory.ReadArraySafeAsync<nint>(arrayPointer, 2))
            .Where(it => !it.IsNullPointer())
            .ToArray();

        if (piercingBindPtrs.Length == 0)
        {
            MaxPiercingBindTimer = 0.0f;
            PiercingBindTimer = 0.0f;
            return;
        }

        float maxPiercingBindTimer = piercingBindPtrs.Select(async it => await Memory.ReadAsync<MHRPiercingBindStructure>(it))
            .AwaitResults()
            .Max(it => it.Timer);

        MaxPiercingBindTimer = Math.Max(MaxPiercingBindTimer, maxPiercingBindTimer);
        PiercingBindTimer = maxPiercingBindTimer;
    }

    public override void Dispose()
    {
        new IDisposable[]
        {
            _onDemonBuildUpChange,
            _onArchDemonModeStateChange,
            _onDemonModeStateChange,
            _onPiercingBindTimerChange
        }.DisposeAll();

        base.Dispose();
    }
}