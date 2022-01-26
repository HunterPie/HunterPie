using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Environment;
using System;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRMonsterPart : IMonsterPart, IEventDispatcher
    {
        private float _health;

        public string Id { get; }

        public float Health
        {
            get => _health;
            private set
            {
                if (value != _health)
                {
                    _health = value;
                    this.Dispatch(OnHealthUpdate, this);
                }
            }
        }

        public float MaxHealth { get; private set; }

        public event EventHandler<IMonsterPart> OnHealthUpdate;
        public event EventHandler<IMonsterPart> OnBreakCountUpdate;

        public MHRMonsterPart(string id)
        {
            Id = id;
        }

        internal void UpdateHealth(float health, float maxHealth)
        {
            MaxHealth = maxHealth;
            Health = health;
        }
    }
}
