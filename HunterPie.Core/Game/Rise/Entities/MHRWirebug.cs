using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Rise.Definitions;
using System;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRWirebug : IEventDispatcher, IUpdatable<MHRWirebugStructure>, IUpdatable<MHRWirebugExtrasStructure>
    {
        private double _timer;
        private double _cooldown;
        private bool _isAvailable = true;

        public long Address { get; internal set; }
        public double Timer
        {
            get => _timer;
            private set
            {
                if (value != _timer)
                {
                    _timer = value;
                    this.Dispatch(OnTimerUpdate, this);
                }
            }
        }
        public double MaxTimer { get; private set; }

        public double Cooldown
        {
            get => _cooldown;
            private set
            {
                if (value != _cooldown)
                {
                    _cooldown = value;
                    this.Dispatch(OnCooldownUpdate, this);
                }
            }
        }
        public double MaxCooldown { get; private set; }

        public bool IsAvailable
        {
            get => _isAvailable;
            private set
            {
                if (value != _isAvailable)
                {
                    _isAvailable = value;
                    this.Dispatch(OnAvailable, this);
                }
            }
        }

        public event EventHandler<MHRWirebug> OnTimerUpdate;
        public event EventHandler<MHRWirebug> OnCooldownUpdate;
        public event EventHandler<MHRWirebug> OnAvailable;

        void IUpdatable<MHRWirebugStructure>.Update(MHRWirebugStructure data)
        {
            MaxCooldown = data.MaxCooldown;
            Cooldown = data.Cooldown;
        }

        void IUpdatable<MHRWirebugExtrasStructure>.Update(MHRWirebugExtrasStructure data)
        {
            MaxTimer = Math.Max(MaxTimer, data.Timer);
            Timer = data.Timer;
            IsAvailable = data.Timer > 0;
        }
    }
}
