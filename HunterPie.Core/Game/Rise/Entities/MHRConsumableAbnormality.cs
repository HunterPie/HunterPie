using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Rise.Definitions;
using System;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRConsumableAbnormality : IAbnormality, IUpdatable<MHRConsumableStructure>, IEventDispatcher
    {
        private float _timer;

        public string Id { get; private set; }
        public string Icon { get; private set; }
        public AbnormalityType Type => AbnormalityType.Consumable;

        public float Timer
        {
            get => _timer;
            private set
            {
                if (_timer != value)
                {
                    _timer = value;
                    this.Dispatch(OnTimerUpdate, this);
                }
            }
        }

        public float MaxTimer { get; private set; }
        public bool IsInfinite { get; private set; }
        public int Level { get; private set; }

        public bool IsBuildup { get; private set; }

        public event EventHandler<IAbnormality> OnTimerUpdate;

        public MHRConsumableAbnormality(AbnormalitySchema data)
        {
            Id = data.Id;
            Icon = data.Icon;

            if (IsBuildup)
                MaxTimer = data.MaxBuildup;
        }

        void IUpdatable<MHRConsumableStructure>.Update(MHRConsumableStructure data)
        {
            MaxTimer = Math.Max(MaxTimer, data.Timer);
            Timer = data.Timer;
        }
    }
}
