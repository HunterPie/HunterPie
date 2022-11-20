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

    public string Id { get; private set; }
    public int Counter
    {
        get => _counter;
        private set
        {
            if (value != _counter)
            {
                _counter = value;
                this.Dispatch(OnCounterUpdate, this);
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
                this.Dispatch(OnTimerUpdate, this);
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
                this.Dispatch(OnBuildUpUpdate, this);
            }
        }
    }
    public float MaxBuildUp { get; private set; }

    public event EventHandler<IMonsterAilment> OnTimerUpdate;
    public event EventHandler<IMonsterAilment> OnBuildUpUpdate;
    public event EventHandler<IMonsterAilment> OnCounterUpdate;

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
