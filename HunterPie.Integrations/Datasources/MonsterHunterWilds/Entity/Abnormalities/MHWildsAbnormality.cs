using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Abnormalities.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Abnormalities;

public sealed class MHWildsAbnormality(
    AbnormalityDefinition definition,
    AbnormalityType type) : CommonAbnormality, IUpdatable<UpdateAbnormalityData>
{
    public override string Id { get; protected set; } = definition.Id;
    public override string Name { get; protected set; } = definition.Name;
    public override string Icon { get; protected set; } = definition.Icon;
    public override bool IsBuildup { get; protected set; } = definition.IsBuildup;
    public override bool IsInfinite { get; protected set; } = definition.IsInfinite;
    public override AbnormalityType Type { get; protected set; } = type;

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

    public void Update(UpdateAbnormalityData data)
    {
        MaxTimer = data.ShouldInferMaxTimer
            ? Math.Max(data.Timer, MaxTimer)
            : data.MaxTimer;
        Timer = data.Timer;
    }
}