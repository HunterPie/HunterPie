using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;

public sealed class MHWAbnormality : CommonAbnormality, IUpdatable<MHWAbnormalityStructure>
{
    private float _timer;

    public override string Id { get; protected set; }

    public override string Name { get; protected set; }

    public override string Icon { get; protected set; }

    public override AbnormalityType Type { get; protected set; }

    public override float Timer
    {
        get => _timer;
        protected set
        {
            if (value != _timer)
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

    public MHWAbnormality(AbnormalitySchema schema)
    {
        Id = schema.Id;
        Name = schema.Name;
        Icon = schema.Icon;
        Type = schema.Category switch
        {
            AbnormalityData.Consumables => AbnormalityType.Consumable,
            AbnormalityData.Songs => AbnormalityType.Song,
            AbnormalityData.Debuffs => AbnormalityType.Debuff,
            AbnormalityData.Skills => AbnormalityType.Skill,
            AbnormalityData.Orchestra => AbnormalityType.Orchestra,
            AbnormalityData.Gears => AbnormalityType.Gear,
            AbnormalityData.Foods => AbnormalityType.Food,
            _ => throw new NotImplementedException("unreachable")
        };
        IsBuildup = schema.IsBuildup;
        IsInfinite = schema.IsInfinite;

        if (IsBuildup)
            MaxTimer = schema.MaxBuildup;
    }

    void IUpdatable<MHWAbnormalityStructure>.Update(MHWAbnormalityStructure data)
    {
        MaxTimer = Math.Max(MaxTimer, data.Timer);
        Timer = data.Timer;
    }
}
