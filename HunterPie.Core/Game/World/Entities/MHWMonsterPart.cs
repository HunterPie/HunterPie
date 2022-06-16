using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.World.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.World.Entities
{
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
                    this.Dispatch(OnFlinchUpdate, this);
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
                    this.Dispatch(OnSeverUpdate, this);
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
                    this.Dispatch(OnTenderizeUpdate, this);
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
                    this.Dispatch(OnBreakCountUpdate, this);
                }
            }
        }
        public PartType Type { get; private set; }

        public event EventHandler<IMonsterPart> OnHealthUpdate;
        public event EventHandler<IMonsterPart> OnTenderizeUpdate;
        public event EventHandler<IMonsterPart> OnFlinchUpdate;
        public event EventHandler<IMonsterPart> OnSeverUpdate;
        public event EventHandler<IMonsterPart> OnBreakCountUpdate;

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

        public bool HasTenderizeId(uint id)
        {
            return _tenderizeIds.Contains(id);
        }

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
}
