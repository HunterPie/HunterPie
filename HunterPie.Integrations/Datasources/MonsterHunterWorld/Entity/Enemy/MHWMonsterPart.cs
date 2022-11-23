using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enemy;

public class MHWMonsterPart :
    IMonsterPart, IEventDispatcher,
    IUpdatable<MHWMonsterPartStructure>,
    IUpdatable<MHWTenderizeInfoStructure>
{
    private float _flinch;
    private float _sever;
    private float _tenderize;
    private int _count;
    private readonly HashSet<uint> _tenderizeIds;

    public string Id { get; }

    public float Health => 0;

    public float MaxHealth => 0;

    public float Flinch
    {
        get => _flinch;
        private set
        {
            if (value != _flinch)
            {
                _flinch = value;
                this.Dispatch(_onFlinchUpdate, this);
            }
        }
    }

    public float MaxFlinch { get; private set; }

    public float Sever
    {
        get => _sever;
        private set
        {
            if (value != _sever)
            {
                _sever = value;
                this.Dispatch(_onSeverUpdate, this);
            }
        }
    }

    public float MaxSever { get; private set; }

    public float Tenderize
    {
        get => _tenderize;
        private set
        {
            if (value != _tenderize)
            {
                _tenderize = value;
                this.Dispatch(_onTenderizeUpdate, this);
            }
        }
    }

    public float MaxTenderize { get; private set; }
    public int Count
    {
        get => _count;
        private set
        {
            if (value != _count)
            {
                _count = value;
                this.Dispatch(_onBreakCountUpdate, this);
            }
        }
    }
    public PartType Type { get; private set; }

    private readonly SmartEvent<IMonsterPart> _onHealthUpdate = new();
    public event EventHandler<IMonsterPart> OnHealthUpdate
    {
        add => _onHealthUpdate.Hook(value);
        remove => _onHealthUpdate.Unhook(value);
    }

    private readonly SmartEvent<IMonsterPart> _onBreakCountUpdate = new();
    public event EventHandler<IMonsterPart> OnBreakCountUpdate
    {
        add => _onBreakCountUpdate.Hook(value);
        remove => _onBreakCountUpdate.Unhook(value);
    }

    private readonly SmartEvent<IMonsterPart> _onTenderizeUpdate = new();
    public event EventHandler<IMonsterPart> OnTenderizeUpdate
    {
        add => _onTenderizeUpdate.Hook(value);
        remove => _onTenderizeUpdate.Unhook(value);
    }

    private readonly SmartEvent<IMonsterPart> _onFlinchUpdate = new();
    public event EventHandler<IMonsterPart> OnFlinchUpdate
    {
        add => _onFlinchUpdate.Hook(value);
        remove => _onFlinchUpdate.Unhook(value);
    }

    private readonly SmartEvent<IMonsterPart> _onSeverUpdate = new();
    public event EventHandler<IMonsterPart> OnSeverUpdate
    {
        add => _onSeverUpdate.Hook(value);
        remove => _onSeverUpdate.Unhook(value);
    }

    private readonly SmartEvent<IMonsterPart> _onPartTypeChange = new();
    public event EventHandler<IMonsterPart> OnPartTypeChange
    {
        add => _onPartTypeChange.Hook(value);
        remove => _onPartTypeChange.Unhook(value);
    }

    public MHWMonsterPart(
        string id,
        bool isSeverable,
        uint[] tenderizeIds
    )
    {
        Id = id;

        Type = isSeverable ? PartType.Severable : PartType.Flinch;
        _tenderizeIds = tenderizeIds.ToHashSet();
    }

    public bool HasTenderizeId(uint id) => _tenderizeIds.Contains(id);

    void IUpdatable<MHWMonsterPartStructure>.Update(MHWMonsterPartStructure data)
    {
        switch (Type)
        {
            case PartType.Severable:
                {
                    MaxSever = data.MaxHealth;
                    Sever = data.Health;
                }

                break;
            case PartType.Flinch:
                {
                    MaxFlinch = data.MaxHealth;
                    Flinch = data.Health;
                }

                break;
            case PartType.Invalid:
            case PartType.Breakable:
            case PartType.Qurio:
            default:
                break;
        }

        Count = data.Counter;
    }

    void IUpdatable<MHWTenderizeInfoStructure>.Update(MHWTenderizeInfoStructure data)
    {
        Tenderize = data.Duration;
        MaxTenderize = data.MaxDuration;
    }
}
