using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using System;

namespace HunterPie.Core.Game.World.Entities
{
    public class MHWMonster : IMonster
    {
        public float Health => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public int Id => throw new NotImplementedException();

        public float MaxHealth => throw new NotImplementedException();

        public bool IsTarget => throw new NotImplementedException();

        public Target Target => throw new NotImplementedException();

        public IMonsterPart[] Parts => throw new NotImplementedException();

        public IMonsterAilment[] Ailments => throw new NotImplementedException();

        public Crown Crown => throw new NotImplementedException();

        public bool IsEnraged => throw new NotImplementedException();

        public IMonsterAilment Enrage => throw new NotImplementedException();

        public float Stamina => throw new NotImplementedException();

        public float MaxStamina => throw new NotImplementedException();

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
        public event EventHandler<EventArgs> OnTargetChange;
        public event EventHandler<IMonsterPart> OnNewPartFound;
        public event EventHandler<IMonsterAilment> OnNewAilmentFound;
        public event EventHandler<EventArgs> OnEnrageStateChange;
    }
}
