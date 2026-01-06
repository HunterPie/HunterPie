using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;

public class MHWildsPlayerStatus : IPlayerStatus, IEventDispatcher, IUpdatable<UpdatePlayerStatus>, IDisposable
{
    private double _affinity;
    public double Affinity
    {
        get => _affinity;
        private set
        {
            if (value.Equals(_affinity))
                return;

            double oldValue = _affinity;
            _affinity = value;

            this.Dispatch(
                toDispatch: _affinityChanged,
                data: new SimpleValueChangeEventArgs<double>(oldValue, value)
            );
        }
    }

    private double _rawDamage;
    public double RawDamage
    {
        get => _rawDamage;
        private set
        {
            if (value == _rawDamage)
                return;

            double oldValue = _rawDamage;
            _rawDamage = value;

            this.Dispatch(
                toDispatch: _rawDamageChanged,
                data: new SimpleValueChangeEventArgs<double>(oldValue, value)
            );
        }
    }

    private double _elementalDamage;
    public double ElementalDamage
    {
        get => _elementalDamage;
        private set
        {
            if (value == _elementalDamage)
                return;

            double oldValue = _elementalDamage;
            _elementalDamage = value;

            this.Dispatch(
                toDispatch: _elementalDamageChanged,
                data: new SimpleValueChangeEventArgs<double>(oldValue, value)
            );
        }
    }

    private readonly SmartEvent<SimpleValueChangeEventArgs<double>> _affinityChanged = new();
    public event EventHandler<SimpleValueChangeEventArgs<double>> AffinityChanged
    {
        add => _affinityChanged.Hook(value);
        remove => _affinityChanged.Unhook(value);
    }

    private readonly SmartEvent<SimpleValueChangeEventArgs<double>> _rawDamageChanged = new();
    public event EventHandler<SimpleValueChangeEventArgs<double>> RawDamageChanged
    {
        add => _rawDamageChanged.Hook(value);
        remove => _rawDamageChanged.Unhook(value);
    }

    private readonly SmartEvent<SimpleValueChangeEventArgs<double>> _elementalDamageChanged = new();
    public event EventHandler<SimpleValueChangeEventArgs<double>> ElementalDamageChanged
    {
        add => _elementalDamageChanged.Hook(value);
        remove => _elementalDamageChanged.Unhook(value);
    }

    public void Update(UpdatePlayerStatus data)
    {
        Affinity = data.Affinity;
        RawDamage = data.RawDamage;
        ElementalDamage = data.ElementalDamage;
    }

    public void Dispose()
    {
        _affinityChanged.Dispose();
        _rawDamageChanged.Dispose();
        _elementalDamageChanged.Dispose();
    }
}