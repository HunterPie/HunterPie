using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Logger;
using System;

namespace HunterPie.Core.Game.World.Entities
{
    public class MHWMonster : Scannable, IMonster, IEventDispatcher
    {
        #region Private
        private readonly long _address;
        private int _id;
        private float _health = -1;
        private bool _isTarget;
        private bool _isEnraged;
        private Target _target;
        private Crown _crown;
        private float _stamina;
        private MHWMonsterAilment _enrage = new MHWMonsterAilment("STATUS_ENRAGE");
        #endregion

        public int Id
        {
            get => _id;
            private set
            {
                if (value != _id)
                {
                    _id = value;
                    this.Dispatch(OnSpawn);
                }
            }
        }

        public string Em { get; private set; }

        public string Name => MHWContext.Strings.GetMonsterNameById(Id);

        public float Health
        {
            get => _health;
            private set
            {
                if (value != _health)
                {
                    _health = value;
                    this.Dispatch(OnHealthChange);

                    if (Health <= 0)
                        this.Dispatch(OnDeath);
                }
            }
        }

        public float MaxHealth { get; private set; }

        public float Stamina
        {
            get => _stamina;
            private set
            {
                if (value != _stamina)
                {
                    _stamina = value;
                    this.Dispatch(OnStaminaChange);
                }
            }
        }

        public float MaxStamina { get; private set; }

        public bool IsTarget
        {
            get => _isTarget;
            private set
            {
                if (_isTarget != value)
                {
                    _isTarget = value;
                    this.Dispatch(OnTarget);
                }
            }
        }

        public IMonsterPart[] Parts => Array.Empty<IMonsterPart>();

        public IMonsterAilment[] Ailments => Array.Empty<IMonsterAilment>();
        public Target Target
        {
            get => _target;
            private set
            {
                if (_target != value)
                {
                    _target = value;
                    this.Dispatch(OnTargetChange);
                }
            }
        }

        public Crown Crown
        {
            get => _crown;
            private set
            {
                if (_crown != value)
                {
                    _crown = value;
                    this.Dispatch(OnCrownChange);
                }
            }
        }

        public bool IsEnraged
        {
            get => _isEnraged;
            private set
            {
                if (value != _isEnraged)
                {
                    _isEnraged = value;
                    this.Dispatch(OnEnrageStateChange);
                }
            }
        }

        public IMonsterAilment Enrage => _enrage;

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

        public MHWMonster(IProcessManager process, long address, string em) : base(process)
        {
            _address = address;
            Em = em;

            Log.Debug($"Initialized monster at address {address:X}");
        }

        [ScannableMethod]
        private void GetMonsterBasicInformation()
        {
            int monsterId = _process.Memory.Read<int>(_address + 0x12280);

            Id = monsterId;
        }

        [ScannableMethod]
        private void GetMonsterHealthData()
        {
            long monsterHealthPtr = _process.Memory.Read<long>(_address + 0x7670);
            float[] healthValues = _process.Memory.Read<float>(monsterHealthPtr + 0x60, 2);

            MaxHealth = healthValues[0];
            Health = healthValues[1];
        }

        [ScannableMethod]
        private void GetMonsterStaminaData()
        {
            float[] staminaValues = _process.Memory.Read<float>(_address + 0x1C0F0, 2);

            MaxStamina = staminaValues[1];
            Stamina = staminaValues[0];
        }

        [ScannableMethod]
        private void GetMonsterCrownData()
        {
            float sizeModifier = _process.Memory.Read<float>(_address + 0x7730);
            float sizeMultiplier = _process.Memory.Read<float>(_address + 0x188);
            
            if (sizeModifier <= 0 || sizeModifier >= 2)
                sizeModifier = 1;

            float monsterSizeMultiplier = sizeMultiplier / sizeModifier;

            MonsterSizeSchema? crownData = MonsterData.GetMonsterData(Id)?.Size;

            if (crownData is null)
                return;

            MonsterSizeSchema crown = crownData.Value;

            if (monsterSizeMultiplier >= crown.Gold)
                Crown = Crown.Gold;
            else if (monsterSizeMultiplier >= crown.Silver)
                Crown = Crown.Silver;
            else if (monsterSizeMultiplier <= crown.Mini)
                Crown = Crown.Mini;
            else
                Crown = Crown.None;
        }
    }
}
