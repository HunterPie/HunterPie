﻿using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Definitions;
using System;

namespace HunterPie.Core.Game.Rise.Entities;

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
                this.Dispatch(OnHealthUpdate, this);
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
                this.Dispatch(OnFlinchUpdate, this);
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
                this.Dispatch(OnSeverUpdate, this);
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
                this.Dispatch(OnQurioHealthChange, this);
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
                this.Dispatch(OnPartTypeChange, this);
            }
        }
    }

    public int Count => 0;

    public event EventHandler<IMonsterPart> OnHealthUpdate;
    public event EventHandler<IMonsterPart> OnQurioHealthChange;
    public event EventHandler<IMonsterPart> OnBreakCountUpdate;
    public event EventHandler<IMonsterPart> OnTenderizeUpdate;
    public event EventHandler<IMonsterPart> OnFlinchUpdate;
    public event EventHandler<IMonsterPart> OnSeverUpdate;
    public event EventHandler<IMonsterPart> OnPartTypeChange;

    public MHRMonsterPart(string id, MHRPartStructure structure)
    {
        Id = id;

        GetCurrentType(structure);
    }

    void IUpdatable<MHRPartStructure>.Update(MHRPartStructure data)
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

    void IUpdatable<MHRQurioPartData>.Update(MHRQurioPartData data)
    {
        if (!data.IsInQurioState && Type != PartType.Qurio)
            return;

        if (!data.IsInQurioState && Type == PartType.Qurio)
        {
            _isInQurio = false;
            return;
        }

        Type = PartType.Qurio;
        QurioMaxHealth = Math.Max(data.Health, QurioMaxHealth);
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
