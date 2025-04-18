using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Abnormalities.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Abnormalities;

public sealed class MHWildsAbnormality : CommonAbnormality, IUpdatable<UpdateAbnormalityData>
{
    public override string Id { get; protected set; }
    public override string Name { get; protected set; }
    public override string Icon { get; protected set; }
    public override bool IsBuildup { get; protected set; }
    public override bool IsInfinite { get; protected set; }
    public override AbnormalityType Type { get; protected set; }

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

    public override int Level { get; protected set; }

    public MHWildsAbnormality(
        AbnormalityDefinition definition,
        AbnormalityType type)
    {
        Type = type;
        Id = definition.Id;
        Name = definition.Name;
        Icon = definition.Icon;
        IsBuildup = definition.IsBuildup;
        IsInfinite = definition.IsInfinite;
    }

    public void Update(UpdateAbnormalityData data)
    {
        MaxTimer = data.ShouldInferMaxTimer
            ? Math.Max(data.Timer, MaxTimer)
            : data.MaxTimer;
        Timer = data.Timer;
    }
}