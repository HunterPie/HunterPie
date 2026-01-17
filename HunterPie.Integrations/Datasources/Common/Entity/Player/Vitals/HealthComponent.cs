using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.Common.Definition;

namespace HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;

public class HealthComponent : IHealthComponent, IEventDispatcher, IUpdatable<HealthData>, IDisposable
{
    public double Current
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onHealthChange, new HealthChangeEventArgs(this));
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
                this.Dispatch(_onHealthChange, new HealthChangeEventArgs(this));
            }
        }
    }


    public double Heal
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onHeal, new HealthChangeEventArgs(this));
            }
        }
    }

    public double RecoverableHealth
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onHealthChange, new HealthChangeEventArgs(this));
            }
        }
    }

    public double MaxPossibleHealth
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onHealthChange, new HealthChangeEventArgs(this));
            }
        }
    }

    protected readonly SmartEvent<HealthChangeEventArgs> _onHealthChange = new();
    public event EventHandler<HealthChangeEventArgs> OnHealthChange
    {
        add => _onHealthChange.Hook(value);
        remove => _onHealthChange.Unhook(value);
    }

    protected readonly SmartEvent<HealthChangeEventArgs> _onHeal = new();
    public event EventHandler<HealthChangeEventArgs> OnHeal
    {
        add => _onHeal.Hook(value);
        remove => _onHeal.Unhook(value);
    }


    public void Update(HealthData data)
    {
        Max = data.MaxHealth;
        Current = data.Health;
        RecoverableHealth = data.RecoverableHealth;
        MaxPossibleHealth = data.MaxPossibleHealth;
        Heal = data.Heal;
    }

    public virtual void Dispose()
    {
        IDisposable[] events = { _onHealthChange, _onHeal };

        events.DisposeAll();
    }
}