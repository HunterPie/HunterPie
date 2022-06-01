using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.World.Definitions;
using System;

namespace HunterPie.Core.Game.World.Entities.Player
{
    public class MHWSpecializedTool : IEventDispatcher, IUpdatable<MHWSpecializedToolStructure>
    {
        private SpecializedToolType _id;
        private float _cooldown;
        private float _timer;

        public SpecializedToolType Id
        {
            get => _id;
            set
            {
                if (value != _id)
                {
                    _id = value;
                    this.Dispatch(OnChange);
                }
            }
        }

        public float Cooldown
        {
            get => _cooldown;
            set
            {
                if (value != _cooldown)
                {
                    _cooldown = value;
                    this.Dispatch(OnCooldownUpdate);
                }
            }
        }

        public float MaxCooldown { get; private set; }

        public float Timer
        {
            get => _timer;
            set
            {
                if (value != _timer)
                {
                    _timer = value;
                    this.Dispatch(OnTimerUpdate);
                }
            }
        }

        public float MaxTimer { get; private set; }

        public event EventHandler<EventArgs> OnCooldownUpdate;
        public event EventHandler<EventArgs> OnTimerUpdate;
        public event EventHandler<EventArgs> OnChange;

        void IUpdatable<MHWSpecializedToolStructure>.Update(MHWSpecializedToolStructure data)
        {
            Id = data.Id;
            MaxTimer = data.MaxTimer;
            Timer = data.Timer;
            MaxCooldown = data.MaxCooldown;
            Cooldown = data.Cooldown;
        }
    }
}
