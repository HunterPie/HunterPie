using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;

public class MHWildsSpecializedTool : ISpecializedTool, IEventDispatcher, IUpdatable<UpdateSpecializedTool>, IDisposable
{
    public SpecializedToolType Id
    {
        get;
        set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onChange, this);
            }
        }
    }

    public float Cooldown
    {
        get;
        set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onCooldownUpdate, this);
            }
        }
    }

    public float MaxCooldown { get; private set; }

    public float Timer
    {
        get;
        set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }

    public float MaxTimer { get; private set; }

    private readonly SmartEvent<ISpecializedTool> _onCooldownUpdate = new();
    public event EventHandler<ISpecializedTool> OnCooldownUpdate
    {
        add => _onCooldownUpdate.Hook(value);
        remove => _onCooldownUpdate.Unhook(value);
    }

    private readonly SmartEvent<ISpecializedTool> _onTimerUpdate = new();
    public event EventHandler<ISpecializedTool> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    private readonly SmartEvent<ISpecializedTool> _onChange = new();
    public event EventHandler<ISpecializedTool> OnChange
    {
        add => _onChange.Hook(value);
        remove => _onChange.Unhook(value);
    }

    public void Update(UpdateSpecializedTool data)
    {
        Id = data.Type;

        float timer = data.IsTimerActive ? data.Timer : 0;
        float cooldown = data.IsTimerActive ? 0 : data.Timer;

        MaxCooldown = data.MaxCooldown;
        Cooldown = cooldown;
        MaxTimer = data.MaxTimer;
        Timer = timer;
    }

    public void Dispose()
    {
        _onCooldownUpdate.Dispose();
        _onTimerUpdate.Dispose();
        _onChange.Dispose();
    }
}