using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Rise.Definitions;
using System;

namespace HunterPie.Core.Game.Rise.Entities
{
    // TODO: Make this a generic Abnormality
    public class MHRDebuffAbnormality : IAbnormality, IUpdatable<MHRDebuffStructure>, IEventDispatcher
    {
        private float _timer;

        public string Id { get; private set; }
        public string Icon { get; private set; }
        public AbnormalityType Type { get; private set; }
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

        public MHRDebuffAbnormality(AbnormalitySchema data)
        {
            Id = data.Id;
            Icon = data.Icon;
            Type = data.Group.StartsWith("Debuffs")
                ? AbnormalityType.Debuff 
                : AbnormalityType.Skill;
            IsBuildup = data.IsBuildup;

            if (IsBuildup)
                MaxTimer = data.MaxBuildup;
        }

        void IUpdatable<MHRDebuffStructure>.Update(MHRDebuffStructure structure)
        {
            MaxTimer = Math.Max(MaxTimer, structure.Timer);
            Timer = structure.Timer;
        }
    }
}
