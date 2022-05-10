using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.World.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.World.Entities
{
    public class MHWMonsterPart : IMonsterPart, IEventDispatcher, IUpdatable<MHWMonsterPartStructure>
    {
        private float _flinch;
        private int _count;

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

        public float Sever => 0;

        public float MaxSever => 0;

        public float Tenderize => throw new NotImplementedException();

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

        public MHWMonsterPart(string id)
        {
            Id = id;

            Type = PartType.Flinch;
        }

        void IUpdatable<MHWMonsterPartStructure>.Update(MHWMonsterPartStructure data)
        {
            Flinch = data.Health;
            MaxFlinch = data.MaxHealth;
            Count = data.Counter;
        }
    }
}
