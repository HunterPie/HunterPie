using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;

public class MHRLongSword(
    IGameProcess process,
    IScanService scanService) : MHRMeleeWeapon(process, scanService, Weapon.Longsword), ILongSword
{
    public int SpiritLevel
    {
        get;
        private set
        {
            if (field == value)
                return;

            int temp = field;
            field = value;
            this.Dispatch(_onSpiritLevelChange, new SimpleValueChangeEventArgs<int>(temp, value));
        }
    }

    public float SpiritBuildUp
    {
        get;
        private set
        {
            if (field.Equals(value))
                return;

            field = value;
            this.Dispatch(_onSpiritBuildUpChange, new BuildUpChangeEventArgs(value, MaxSpiritBuildUp));
        }
    }

    public float MaxSpiritBuildUp => 100.0f;

    public float SpiritRegenerationTimer
    {
        get;
        private set
        {
            if (field.Equals(value))
                return;

            field = value;
            this.Dispatch(_onSpiritRegenerationChange, new TimerChangeEventArgs(value, MaxSpiritRegenerationTimer));
        }
    }

    public float MaxSpiritRegenerationTimer { get; private set; }

    public float SpiritLevelTimer
    {
        get;
        private set
        {
            if (field.Equals(value))
                return;

            field = value;
            this.Dispatch(_onSpiritLevelTimerChange, new TimerChangeEventArgs(value, MaxSpiritLevelTimer));
        }
    }

    public float MaxSpiritLevelTimer { get; private set; }

    #region Events
    private readonly SmartEvent<SimpleValueChangeEventArgs<int>> _onSpiritLevelChange = new();
    public event EventHandler<SimpleValueChangeEventArgs<int>> OnSpiritLevelChange
    {
        add => _onSpiritLevelChange.Hook(value);
        remove => _onSpiritLevelChange.Unhook(value);
    }

    private readonly SmartEvent<BuildUpChangeEventArgs> _onSpiritBuildUpChange = new();
    public event EventHandler<BuildUpChangeEventArgs> OnSpiritBuildUpChange
    {
        add => _onSpiritBuildUpChange.Hook(value);
        remove => _onSpiritBuildUpChange.Unhook(value);
    }

    private readonly SmartEvent<TimerChangeEventArgs> _onSpiritRegenerationChange = new();
    public event EventHandler<TimerChangeEventArgs> OnSpiritRegenerationChange
    {
        add => _onSpiritRegenerationChange.Hook(value);
        remove => _onSpiritRegenerationChange.Unhook(value);
    }

    private readonly SmartEvent<TimerChangeEventArgs> _onSpiritLevelTimerChange = new();
    public event EventHandler<TimerChangeEventArgs> OnSpiritLevelTimerChange
    {
        add => _onSpiritLevelTimerChange.Hook(value);
        remove => _onSpiritLevelTimerChange.Unhook(value);
    }

    #endregion

    [ScannableMethod]
    private async Task GetData()
    {
        MHRLongSwordStructure structure = await Memory.DerefAsync<MHRLongSwordStructure>(
            address: AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("CURRENT_WEAPON_OFFSETS")
        );

        float[] maxTimersByLevel = await Memory.ReadArraySafeAsync<float>(
            address: structure.LevelMaxTimersPointer,
            size: 4
        );
        float maxLevelTimer = maxTimersByLevel.ElementAtOrDefault(
            index: structure.Level
        ).ToAbnormalitySeconds();
        float currentLevelTimer = structure.LevelTimer.ToAbnormalitySeconds();
        float regenerationTimer = structure.BuildUpBuffTimer.ToAbnormalitySeconds();

        SpiritBuildUp = structure.GaugeBuildUp;
        MaxSpiritLevelTimer = maxLevelTimer;
        SpiritLevelTimer = currentLevelTimer;
        SpiritLevel = structure.Level;
        MaxSpiritRegenerationTimer = Math.Max(regenerationTimer, MaxSpiritRegenerationTimer);
        SpiritRegenerationTimer = regenerationTimer;
    }

    public override void Dispose()
    {
        IDisposableExtensions.DisposeAll(
            _onSpiritLevelChange,
            _onSpiritBuildUpChange,
            _onSpiritRegenerationChange,
            _onSpiritLevelTimerChange
        );

        base.Dispose();
    }
}