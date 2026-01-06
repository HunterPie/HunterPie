using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enemy;

public sealed class MHWMonsterPart :
    CommonPart,
    IUpdatable<MHWMonsterPartStructure>,
    IUpdatable<MHWTenderizeInfoStructure>
{
    private MonsterPartDefinition _definition;

    private readonly HashSet<uint> _tenderizeIds;

    public override string Id { get; protected set; }

    private float _health;
    public override float Health
    {
        get => _health;
        protected set
        {
            if (value == _health)
                return;

            _health = value;
            this.Dispatch(_onHealthUpdate, this);
        }
    }

    public override float MaxHealth { get; protected set; }

    private float _flinch;
    public override float Flinch
    {
        get => _flinch;
        protected set
        {
            if (value != _flinch)
            {
                _flinch = value;
                this.Dispatch(_onFlinchUpdate, this);
            }
        }
    }

    public override float MaxFlinch { get; protected set; }

    private float _sever;
    public override float Sever
    {
        get => _sever;
        protected set
        {
            if (value != _sever)
            {
                _sever = value;
                this.Dispatch(_onSeverUpdate, this);
            }
        }
    }

    public override float MaxSever { get; protected set; }

    private float _tenderize;
    public override float Tenderize
    {
        get => _tenderize;
        protected set
        {
            if (value != _tenderize)
            {
                _tenderize = value;
                this.Dispatch(_onTenderizeUpdate, this);
            }
        }
    }

    public override float MaxTenderize { get; protected set; }

    private int _count;
    public override int Count
    {
        get => _count;
        protected set
        {
            if (value != _count)
            {
                _count = value;
                this.Dispatch(_onBreakCountUpdate, this);
            }
        }
    }
    public override PartType Type { get; protected set; }

    public MHWMonsterPart(
        MonsterPartDefinition definition
    ) : base(definition)
    {
        _definition = definition;
        Id = definition.String;

        Type = definition switch
        {
            { IsSeverable: true } => PartType.Severable,
            { BreakThresholds.Count: > 0 } => PartType.Breakable,
            _ => PartType.Flinch
        };
        _tenderizeIds = definition.TenderizeIds.ToHashSet();
    }

    public bool HasTenderizeId(uint id) => _tenderizeIds.Contains(id);

    public void Update(MHWMonsterPartStructure data)
    {
        Count = data.Counter;

        Action<MHWMonsterPartStructure> executor = Type switch
        {
            PartType.Severable => UpdateSeverableData,
            PartType.Flinch => UpdateFlinchData,
            PartType.Breakable => UpdateBreakableData,
            _ => static (_) => { }
        };

        executor(data);
    }

    public void Update(MHWTenderizeInfoStructure data)
    {
        Tenderize = data.Duration;
        MaxTenderize = data.MaxDuration;
    }

    private void UpdateSeverableData(MHWMonsterPartStructure data)
    {
        MaxSever = data.MaxHealth;
        Sever = data.Health;
    }

    private void UpdateFlinchData(MHWMonsterPartStructure data)
    {
        MaxFlinch = data.MaxHealth;
        Flinch = data.Health;
    }

    private void UpdateBreakableData(MHWMonsterPartStructure data)
    {
        int? nextThreshold = GetNextThreshold();

        MaxFlinch = data.MaxHealth;
        Flinch = data.Health;

        if (nextThreshold is int threshold)
        {
            MaxHealth = threshold * data.MaxHealth;
            Health = (Math.Max(0, threshold - Count - 1) * data.MaxHealth) + data.Health;
            return;
        }

        MaxHealth = data.MaxHealth;
        Health = data.MaxHealth;
    }

    private int? GetNextThreshold()
    {
        foreach (int threshold in _definition.BreakThresholds)
        {
            if (threshold > Count)
                return threshold;
        }

        return null;
    }
}