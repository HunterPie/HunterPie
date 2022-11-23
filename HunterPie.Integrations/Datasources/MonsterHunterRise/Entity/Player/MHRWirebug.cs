using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

public class MHRWirebug : IEventDispatcher, IUpdatable<MHRWirebugExtrasStructure>, IUpdatable<MHRWirebugData>
{
    private double _timer;
    private double _cooldown;
    private bool _isAvailable = true;
    private bool _isBlocked;

    public long Address { get; internal set; }
    public double Timer
    {
        get => _timer;
        private set
        {
            if (value != _timer)
            {
                _timer = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }
    public double MaxTimer { get; private set; }

    public double Cooldown
    {
        get => _cooldown;
        private set
        {
            if (value != _cooldown)
            {
                _cooldown = value;
                this.Dispatch(_onCooldownUpdate, this);
            }
        }
    }
    public double MaxCooldown { get; private set; }

    public bool IsAvailable
    {
        get => _isAvailable;
        private set
        {
            if (value != _isAvailable)
            {
                _isAvailable = value;
                this.Dispatch(_onAvailable, this);
            }
        }
    }

    public bool IsBlocked
    {
        get => _isBlocked;
        private set
        {
            if (value != _isBlocked)
            {
                _isBlocked = value;
                this.Dispatch(_onBlockedStateChange, this);
            }
        }
    }

    private readonly SmartEvent<MHRWirebug> _onTimerUpdate = new();
    public event EventHandler<MHRWirebug> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    private readonly SmartEvent<MHRWirebug> _onCooldownUpdate = new();
    public event EventHandler<MHRWirebug> OnCooldownUpdate
    {
        add => _onCooldownUpdate.Hook(value);
        remove => _onCooldownUpdate.Unhook(value);
    }

    private readonly SmartEvent<MHRWirebug> _onAvailable = new();
    public event EventHandler<MHRWirebug> OnAvailable
    {
        add => _onAvailable.Hook(value);
        remove => _onAvailable.Unhook(value);
    }

    private readonly SmartEvent<MHRWirebug> _onBlockedStateChange = new();
    public event EventHandler<MHRWirebug> OnBlockedStateChange
    {
        add => _onBlockedStateChange.Hook(value);
        remove => _onBlockedStateChange.Unhook(value);
    }

    void IUpdatable<MHRWirebugExtrasStructure>.Update(MHRWirebugExtrasStructure data)
    {
        MaxTimer = Math.Max(MaxTimer, data.Timer);
        Timer = data.Timer;
        IsAvailable = data.Timer > 0;
    }

    void IUpdatable<MHRWirebugData>.Update(MHRWirebugData data)
    {
        IsBlocked = data.IsBlocked;
        MaxCooldown = data.Structure.MaxCooldown;
        Cooldown = data.Structure.Cooldown;
    }
}
