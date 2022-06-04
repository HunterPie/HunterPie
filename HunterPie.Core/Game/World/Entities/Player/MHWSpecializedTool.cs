using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.World.Definitions;
using System;

namespace HunterPie.Core.Game.World.Entities.Player
{
    public class MHWSpecializedTool : ISpecializedTool, IEventDispatcher, IUpdatable<MHWSpecializedToolStructure>
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
                    this.Dispatch(OnChange, this);
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
                    this.Dispatch(OnCooldownUpdate, this);
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
                    this.Dispatch(OnTimerUpdate, this);
                }
            }
        }

        public float MaxTimer { get; private set; }

        public event EventHandler<ISpecializedTool> OnCooldownUpdate;
        public event EventHandler<ISpecializedTool> OnTimerUpdate;
        public event EventHandler<ISpecializedTool> OnChange;

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
