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

    private float _sever;
    public override float Sever
    {
        get => _sever;
        protected set
        {
            if (_sever.Equals(value))
                return;

            _sever = value;
            this.Dispatch(
                toDispatch: _onSeverUpdate,
                data: this
            );
        }
    }
    public override float MaxSever { get; protected set; }

    public override float Tenderize { get; protected set; }
    public override float MaxTenderize { get; protected set; }

    private int _count;
    public override int Count
    {
        get => _count;
        protected set
        {
            if (value == _count)
                return;

            _count = value;
            this.Dispatch(
                toDispatch: _onBreakCountUpdate,
                data: this
            );
        }
    }

    private PartType _type;
    public override PartType Type
    {
        get => _type;
        protected set
        {
            if (value == _type)
                return;

            _type = value;
            this.Dispatch(
                toDispatch: _onPartTypeChange,
                data: this
            );
        }
    }

    public MHWildsMonsterPart(MonsterPartDefinition definition) : base(definition)
    {
        Id = definition.String;
        Type = PartType.Flinch;
    }

    public void Update(UpdatePartData data)
    {
        Type = data switch
        {
            { IsSeverable: true } => PartType.Severable,
            { IsBreakable: true } => PartType.Breakable,
            _ => PartType.Flinch
        };

        int normalizedBreakMultiplier = Math.Max(0, data.BreakMultiplier - 1 - data.HealthResetCount);
        if (data.IsSeverable)
        {
            MaxSever = data.MaxHealth * (data.BreakMultiplier * data.MaxBreaks);
            Sever = (data.MaxHealth * normalizedBreakMultiplier) + data.Health;
        }

        if (data.IsBreakable)
        {
            Count = data.Breaks;
            MaxHealth = data.MaxHealth * data.BreakMultiplier;
            Health = data.Breaks >= data.MaxBreaks
                ? MaxHealth
                : (data.MaxHealth * normalizedBreakMultiplier) + data.Health;
        }
        else
            Count = data.HealthResetCount;

        MaxFlinch = data.MaxHealth;
        Flinch = data.Health;
    }
}