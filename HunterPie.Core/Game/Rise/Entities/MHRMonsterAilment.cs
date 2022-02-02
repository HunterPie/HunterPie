using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Environment;
using System;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRMonsterAilment : IMonsterAilment, IEventDispatcher
    {
        private int _counter;
        private float _timer;
        private float _buildup;

        public string Id { get; private set; }
        public int Counter 
        { 
            get => _counter;
            private set
            {
                if (value != _counter)
                {
                    _counter = value;
                    this.Dispatch(OnCounterUpdate, this);
                }
            }
        }
        public float Timer
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
        public float MaxTimer { get; private set; }
        public float BuildUp
        {
            get => _buildup;
            private set
            {
                if (value != _buildup)
                {
                    _buildup = value;
                    this.Dispatch(OnBuildUpUpdate, this);
                }
            } 
        }
        public float MaxBuildUp { get; private set; }

        public event EventHandler<IMonsterAilment> OnTimerUpdate;
        public event EventHandler<IMonsterAilment> OnBuildUpUpdate;
        public event EventHandler<IMonsterAilment> OnCounterUpdate;

        public MHRMonsterAilment(string ailmentId)
        {
            Id = ailmentId;
        }

        public void UpdateInfo(float timer, float maxTimer, float buildup, float maxBuildup, int counter)
        {
            MaxTimer = maxTimer;
            Timer = timer;
            MaxBuildUp = maxBuildup;
            BuildUp = buildup;
            Counter = counter;
        }
    }
}
