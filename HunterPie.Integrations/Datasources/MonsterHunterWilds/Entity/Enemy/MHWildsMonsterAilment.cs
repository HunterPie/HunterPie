using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public sealed class MHWildsMonsterAilment(AilmentDefinition definition) : CommonAilment(definition), IUpdatable<MHWildsAilment>, IUpdatable<MHWildsBuildUp>
{
    private bool _isActive;

    public override string Id { get; protected set; } = definition.String;


    private int _counter;
    public override int Counter
    {
        get => _counter;
        protected set
        {
            if (_counter == value)
                return;

            _counter = value;
            this.Dispatch(
                toDispatch: _onCounterUpdate,
                data: this
            );
        }
    }

    private float _timer;
    public override float Timer
    {
        get => _timer;
        protected set
        {
            if (_timer.Equals(value))
                return;

            _timer = value;
            this.Dispatch(
                toDispatch: _onTimerUpdate,
                data: this
            );
        }
    }

    public override float MaxTimer { get; protected set; }

    private float _buildUp;
    public override float BuildUp
    {
        get => _buildUp;
        protected set
        {
            if (_buildUp.Equals(value))
                return;

            _buildUp = value;
            this.Dispatch(
                toDispatch: _onBuildUpUpdate,
                data: this
            );
        }
    }

    public override float MaxBuildUp { get; protected set; }

    public void Update(MHWildsAilment data)
    {
        bool isActive = data.IsActive == 1;

        if (isActive && !_isActive)
            Counter++;

        MaxTimer = data.Timer.Max;
        Timer = data.IsActive switch
        {
            1 => MaxTimer - data.Timer.Current,
            _ => 0
        };

        _isActive = data.IsActive == 1;
    }

    public void Update(MHWildsBuildUp data)
    {
        MaxBuildUp = data.Max;
        BuildUp = data.Current;
    }
}