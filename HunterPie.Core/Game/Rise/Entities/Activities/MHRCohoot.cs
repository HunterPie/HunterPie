﻿using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Rise.Definitions;
using System;

namespace HunterPie.Core.Game.Rise.Entities.Activities;

public class MHRCohoot : IEventDispatcher, IUpdatable<MHRCohootStructure>
{
    private int _kamuraCount;
    private int _elgadoCount;

    public int KamuraCount
    {
        get => _kamuraCount;
        private set
        {
            if (value != _kamuraCount)
            {
                _kamuraCount = value;
                this.Dispatch(OnKamuraCountChange, this);
            }
        }
    }

    public int ElgadoCount
    {
        get => _elgadoCount;
        private set
        {
            if (value != _elgadoCount)
            {
                _elgadoCount = value;
                this.Dispatch(OnElgadoCountChange, this);
            }
        }
    }

    public int MaxCount { get; private set; } = 5;

    public event EventHandler<MHRCohoot> OnKamuraCountChange;
    public event EventHandler<MHRCohoot> OnElgadoCountChange;

    void IUpdatable<MHRCohootStructure>.Update(MHRCohootStructure data)
    {
        ElgadoCount = data.ElgadoCount;
        KamuraCount = data.KamuraCount;
    }
}
