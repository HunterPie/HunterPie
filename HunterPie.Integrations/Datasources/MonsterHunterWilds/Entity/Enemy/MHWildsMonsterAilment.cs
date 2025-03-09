using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public sealed class MHWildsMonsterAilment : CommonAilment, IUpdatable<MHWildsAilment>, IUpdatable<MHWildsBuildUp>
{
    public override string Id { get; protected set; }

    public override int Counter { get; protected set; }

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

    public MHWildsMonsterAilment(AilmentDefinition definition) : base(definition)
    {
        Id = definition.String;
    }

    public void Update(MHWildsAilment data)
    {
        MaxTimer = data.Timer.Max;

        Timer = data.IsActive switch
        {
            1 => MaxTimer - data.Timer.Current,
            _ => 0
        };
    }

    public void Update(MHWildsBuildUp data)
    {
        MaxBuildUp = data.Max;
        BuildUp = data.Current;
    }
}