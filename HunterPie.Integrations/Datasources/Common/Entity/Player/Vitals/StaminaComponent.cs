using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.Common.Definition;

namespace HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;

public class StaminaComponent : IStaminaComponent, IEventDispatcher, IUpdatable<StaminaData>, IDisposable
{
    public double Current
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onStaminaChange, new StaminaChangeEventArgs(this));
            }
        }
    }

    public double Max
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onStaminaChange, new StaminaChangeEventArgs(this));
            }
        }
    }

    public double MaxRecoverableStamina
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onStaminaChange, new StaminaChangeEventArgs(this));
            }
        }
    }

    public double MaxPossibleStamina
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onStaminaChange, new StaminaChangeEventArgs(this));
            }
        }
    }

    protected readonly SmartEvent<StaminaChangeEventArgs> _onStaminaChange = new();
    public event EventHandler<StaminaChangeEventArgs> OnStaminaChange
    {
        add => _onStaminaChange.Hook(value);
        remove => _onStaminaChange.Unhook(value);
    }

    public void Update(StaminaData data)
    {
        Max = data.MaxStamina;
        Current = data.Stamina;
        MaxRecoverableStamina = data.MaxRecoverableStamina;
        MaxPossibleStamina = data.MaxPossibleStamina;
    }

    public virtual void Dispose()
    {
        _onStaminaChange.Dispose();
    }
}