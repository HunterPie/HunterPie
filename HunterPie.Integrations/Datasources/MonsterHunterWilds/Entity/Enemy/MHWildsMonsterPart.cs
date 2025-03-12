using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public sealed class MHWildsMonsterPart : CommonPart, IUpdatable<UpdatePartData>
{
    public override string Id { get; protected set; }

    private float _health;
    public override float Health
    {
        get => _health;
        protected set
        {
            if (_health.Equals(value))
                return;

            _health = value;
            this.Dispatch(
                toDispatch: _onHealthUpdate,
                data: this
            );
        }
    }

    public override float MaxHealth { get; protected set; }

    private float _flinch;

    public override float Flinch
    {
        get => _flinch;
        protected set
        {
            if (_flinch.Equals(value))
                return;

            _flinch = value;
            this.Dispatch(
                toDispatch: _onFlinchUpdate,
                data: this
            );
        }
    }
    public override float MaxFlinch { get; protected set; }

    public override float Sever { get; protected set; }
    public override float MaxSever { get; protected set; }

    public override float Tenderize { get; protected set; }
    public override float MaxTenderize { get; protected set; }

    public override int Count { get; protected set; }

    public override PartType Type { get; protected set; }

    public MHWildsMonsterPart(MonsterPartDefinition definition) : base(definition)
    {
        Id = definition.String;
        Type = PartType.Flinch;
    }

    public void Update(UpdatePartData data)
    {
        MaxFlinch = data.MaxHealth;
        Flinch = data.Health;
    }
}