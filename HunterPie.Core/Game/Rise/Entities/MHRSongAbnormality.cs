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
    public class MHRSongAbnormality : IAbnormality, IEventDispatcher
    {
        const float SECONDS_MULTIPLIER = 60;

        private float _timer;

        public string Id { get; private set; }
        public string Icon { get; private set; }
        public AbnormalityType Type => AbnormalityType.Song;
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

        public MHRSongAbnormality(int id)
        {
            var data = AbnormalityData.GetSongAbnormalityData(id);

            if (data is AbnormalitySchema schema)
            {
                Id = schema.Name;
                Icon = schema.Icon;
            }
        }

        internal void Update(MHRHHAbnormality data)
        {
            MaxTimer = Math.Max(MaxTimer, data.Timer / SECONDS_MULTIPLIER);
            Timer = data.Timer / SECONDS_MULTIPLIER;
        }
    }
}
