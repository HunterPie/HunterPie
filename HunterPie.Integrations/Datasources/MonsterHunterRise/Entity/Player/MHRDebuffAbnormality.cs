using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

// TODO: Make this a generic Abnormality
public sealed class MHRDebuffAbnormality : CommonAbnormality, IUpdatable<MHRDebuffStructure>
{
    private float _timer;

    public override string Id { get; protected set; }
    public override string Icon { get; protected set; }
    public override AbnormalityType Type { get; protected set; }
    public override float Timer
    {
        get => _timer;
        protected set
        {
            if (_timer != value)
            {
                _timer = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }
    public override float MaxTimer { get; protected set; }
    public override bool IsInfinite { get; protected set; }
    public override int Level { get; protected set; }
    public override bool IsBuildup { get; protected set; }

    public MHRDebuffAbnormality(AbnormalitySchema data)
    {
        Id = data.Id;
        Icon = data.Icon;
        Type = data.Group.StartsWith("Debuffs")
            ? AbnormalityType.Debuff
            : AbnormalityType.Skill;
        IsBuildup = data.IsBuildup;

        if (IsBuildup)
            MaxTimer = data.MaxBuildup;
    }

    void IUpdatable<MHRDebuffStructure>.Update(MHRDebuffStructure structure)
    {
        MaxTimer = Math.Max(MaxTimer, structure.Timer);
        Timer = structure.Timer;
    }
}
