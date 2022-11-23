using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;

public class MHRMonsterPart : IMonsterPart, IEventDispatcher, IUpdatable<MHRPartStructure>, IUpdatable<MHRQurioPartData>
{
    private float _qurioHealth;
    private float _health;
    private float _flinch;
    private float _sever;
    private PartType _type;
    private bool _isInQurio;

    public string Id { get; }

    public float Health
    {
        get => _health;
        private set
        {
            if (value != _health)
            {
                _health = value;
                this.Dispatch(_onHealthUpdate, this);
            }
        }
    }

    public float MaxHealth { get; private set; }

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

    public float Tenderize => 0;
    public float MaxTenderize => 0;

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

    public float QurioHealth
    {
        get => _qurioHealth;
        private set
        {
            if (value != _qurioHealth)
            {
                _qurioHealth = value;
                this.Dispatch(_onQurioHealthChange, this);
            }
        }
    }

    public float QurioMaxHealth { get; private set; }

    public PartType Type
    {
        get => _type;
        private set
        {
            if (value != _type)
            {
                _type = value;
                this.Dispatch(_onPartTypeChange, this);
            }
        }
    }

    public int Count => 0;

    private readonly SmartEvent<IMonsterPart> _onHealthUpdate = new();
    public event EventHandler<IMonsterPart> OnHealthUpdate
    {
        add => _onHealthUpdate.Hook(value);
        remove => _onHealthUpdate.Unhook(value);
    }

    private readonly SmartEvent<IMonsterPart> _onQurioHealthChange = new();
    public event EventHandler<IMonsterPart> OnQurioHealthChange
    {
        add => _onQurioHealthChange.Hook(value);
        remove => _onQurioHealthChange.Unhook(value);
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

    public MHRMonsterPart(string id)
    {
        Id = id;
    }

    public MHRMonsterPart(string id, MHRPartStructure structure)
    {
        Id = id;

        GetCurrentType(structure);
    }

    public void Update(MHRPartStructure data)
    {
        if (Type == PartType.Qurio && !_isInQurio)
            GetCurrentType(data);

        MaxHealth = data.MaxHealth;
        Health = data.Health;
        MaxFlinch = data.MaxFlinch;
        Flinch = data.Flinch;
        MaxSever = data.MaxSever;
        Sever = data.Sever;
    }

    public void Update(MHRQurioPartData data)
    {
        switch (data.IsInQurioState)
        {
            case false when Type != PartType.Qurio:
                return;
            case false when Type == PartType.Qurio:
                _isInQurio = false;
                return;
        }

        Type = PartType.Qurio;
        QurioMaxHealth = Math.Max(data.MaxHealth, Math.Max(data.Health, QurioMaxHealth));
        QurioHealth = data.Health;
        _isInQurio = data.IsInQurioState;
    }

    private void GetCurrentType(MHRPartStructure structure)
    {
        if (structure.MaxSever > 0)
            Type = PartType.Severable;
        else if (structure.MaxHealth > 0)
            Type = PartType.Breakable;
        else if (structure.MaxFlinch > 0)
            Type = PartType.Flinch;
    }
}
