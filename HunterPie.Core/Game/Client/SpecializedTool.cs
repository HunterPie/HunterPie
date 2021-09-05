using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Enums;
using System;
using HunterPie.Core.Extensions;

namespace HunterPie.Core.Game.Client
{
    public class SpecializedTool : IEventDispatcher
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
    }
}
