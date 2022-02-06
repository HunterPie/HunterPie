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
    public class MHRConsumableAbnormality : IAbnormality, IEventDispatcher
    {
        const float SECONDS_MULTIPLIER = 60;

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

        public event EventHandler<IAbnormality> OnTimerUpdate;

        public MHRConsumableAbnormality(int id)
        {
            AbnormalitySchema? schema = AbnormalityData.GetConsumableAbnormalityData(id);

            if (schema is AbnormalitySchema data)
            {
                Id = data.Id;
                Icon = data.Icon;

            }
        }

        internal void Update(MHRConsumableStructure structure)
        {
            MaxTimer = Math.Max(MaxTimer, structure.Timer / SECONDS_MULTIPLIER);
            Timer = structure.Timer / SECONDS_MULTIPLIER;
        }
    }
}
