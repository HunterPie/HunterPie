using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;

public class MHWSpecializedTool : ISpecializedTool, IEventDispatcher, IUpdatable<MHWSpecializedToolStructure>, IDisposable
{
    public SpecializedToolType Id
    {
        get;
        set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(
                toDispatch: _onChange,
                data: this
            );
        }
    }

    public float Cooldown
    {
        get;
        set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(
                toDispatch: _onCooldownUpdate,
                data: this
            );
        }
    }

    public float MaxCooldown { get; private set; }

    public float Timer
    {
        get;
        set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(
                toDispatch: _onTimerUpdate,
                data: this
            );
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

    public void Update(MHWSpecializedToolStructure data)
    {
        Id = data.Id;
        MaxTimer = data.MaxTimer;
        Timer = data.Timer;
        MaxCooldown = data.MaxCooldown;
        Cooldown = data.Cooldown;
    }

    public void Dispose()
    {
        _onCooldownUpdate.Dispose();
        _onTimerUpdate.Dispose();
        _onChange.Dispose();
    }
}