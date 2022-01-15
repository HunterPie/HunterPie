using HunterPie.Core.Game.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.World.Entities
{
    public class MHWMonster : IMonster
    {
        public float Health => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public int Id => throw new NotImplementedException();

        public float MaxHealth => throw new NotImplementedException();

        public event EventHandler<EventArgs> OnSpawn;
        public event EventHandler<EventArgs> OnLoad;
        public event EventHandler<EventArgs> OnDespawn;
        public event EventHandler<EventArgs> OnDeath;
        public event EventHandler<EventArgs> OnCapture;
        public event EventHandler<EventArgs> OnTarget;
        public event EventHandler<EventArgs> OnCrownChange;
        public event EventHandler<EventArgs> OnHealthChange;
        public event EventHandler<EventArgs> OnStaminaChange;
        public event EventHandler<EventArgs> OnActionChange;
        public event EventHandler<EventArgs> OnEnrage;
        public event EventHandler<EventArgs> OnUnenrage;
        public event EventHandler<EventArgs> OnEnrageTimerChange;
    }
}
