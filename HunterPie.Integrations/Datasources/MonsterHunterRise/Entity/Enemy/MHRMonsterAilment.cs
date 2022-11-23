using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;

public class MHRMonsterAilment : IMonsterAilment, IEventDispatcher, IUpdatable<MHRAilmentData>, IUpdatable<MHREnrageStructure>
{
    private int _counter;
    private float _timer;
    private float _buildup;

    public string Id { get; }
    public int Counter
    {
        get => _counter;
        private set
        {
            if (value != _counter)
            {
                _counter = value;
                this.Dispatch(_onCounterUpdate, this);
            }
        }
    }
    public float Timer
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
    public float MaxTimer { get; private set; }
    public float BuildUp
    {
        get => _buildup;
        private set
        {
            if (value != _buildup)
            {
                _buildup = value;
                this.Dispatch(_onBuildUpUpdate, this);
            }
        }
    }
    public float MaxBuildUp { get; private set; }

    private readonly SmartEvent<IMonsterAilment> _onTimerUpdate = new();
    public event EventHandler<IMonsterAilment> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    private readonly SmartEvent<IMonsterAilment> _onBuildUpUpdate = new();
    public event EventHandler<IMonsterAilment> OnBuildUpUpdate
    {
        add => _onBuildUpUpdate.Hook(value);
        remove => _onBuildUpUpdate.Unhook(value);
    }

    private readonly SmartEvent<IMonsterAilment> _onCounterUpdate = new();
    public event EventHandler<IMonsterAilment> OnCounterUpdate
    {
        add => _onCounterUpdate.Hook(value);
        remove => _onCounterUpdate.Unhook(value);
    }

    public MHRMonsterAilment(string ailmentId)
    {
        Id = ailmentId;
    }

    public void Update(MHRAilmentData data)
    {
        MaxTimer = data.MaxTimer;
        Timer = data.Timer;
        MaxBuildUp = data.MaxBuildUp;
        BuildUp = data.BuildUp;
        Counter = data.Counter;
    }

    public void Update(MHREnrageStructure data)
    {
        MaxTimer = data.MaxTimer;
        Timer = data.Timer;
        MaxBuildUp = data.MaxBuildUp;
        BuildUp = data.BuildUp;
        Counter = data.Counter;
    }
}
