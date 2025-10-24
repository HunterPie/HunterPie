using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Activities;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;

public class MHWildsIngredientsCenter : IEventDispatcher, IDisposable, IUpdatable<MHWildsIngredientCenterContext>
{
    private float _timer;
    public float Timer
    {
        get => _timer;
        private set
        {
            if (value.Equals(_timer))
                return;

            _timer = value;
            this.Dispatch(
                toDispatch: _timerChanged,
                data: new TimerChangeEventArgs(value, MaxTimer)
            );
        }
    }

    public float MaxTimer { get; private set; }

    private int _rations;
    public int Rations
    {
        get => _rations;
        private set
        {
            if (value == _rations)
                return;

            _rations = value;
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