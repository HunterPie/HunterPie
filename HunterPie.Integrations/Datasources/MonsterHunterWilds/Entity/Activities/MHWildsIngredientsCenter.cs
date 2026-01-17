using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Activities;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;

public class MHWildsIngredientsCenter : IEventDispatcher, IDisposable, IUpdatable<MHWildsIngredientCenterContext>
{
    public float Timer
    {
        get;
        private set
        {
            if (value.Equals(field))
                return;

            field = value;
            this.Dispatch(
                toDispatch: _timerChanged,
                data: new TimerChangeEventArgs(value, MaxTimer)
            );
        }
    }

    public float MaxTimer { get; private set; }

    public int Rations
    {
        get;
        private set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(
                toDispatch: _rationsChanged,
                data: new CounterChangeEventArgs(value, MaxRations)
            );
        }
    }

    public readonly int MaxRations = 10;

    private readonly SmartEvent<TimerChangeEventArgs> _timerChanged = new();
    public event EventHandler<TimerChangeEventArgs> TimerChanged
    {
        add => _timerChanged.Hook(value);
        remove => _timerChanged.Unhook(value);
    }

    private readonly SmartEvent<CounterChangeEventArgs> _rationsChanged = new();
    public event EventHandler<CounterChangeEventArgs> RationsChanged
    {
        add => _rationsChanged.Hook(value);
        remove => _rationsChanged.Unhook(value);
    }

    public void Dispose()
    {
        _timerChanged.Dispose();
        _rationsChanged.Dispose();
    }

    public void Update(MHWildsIngredientCenterContext data)
    {
        MaxTimer = Math.Max(data.Timer, MaxTimer);
        Timer = data.Timer;
        Rations = data.Count;
    }
}