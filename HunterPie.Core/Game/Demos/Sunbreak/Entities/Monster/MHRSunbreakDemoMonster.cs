using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.DTO.Monster;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Crypto;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Demos.Sunbreak.Entities.Monster
{
    public class MHRSunbreakDemoMonster : Scannable, IMonster, IEventDispatcher
    {
        private readonly long _address;

        private int _id = -1;
        private float _health = -1;
        private bool _isTarget;
        private bool _isEnraged;
        private Target _target;
        private Crown _crown;
        private float _stamina;
        private MHRMonsterAilment _enrage = new MHRMonsterAilment("STATUS_ENRAGE");
        private readonly Dictionary<long, MHRMonsterPart> parts = new();
        private readonly Dictionary<long, MHRMonsterAilment> ailments = new();

        public int Id
        {
            get => _id;
            private set
            {
                if (_id != value)
                {
                    _id = value;
                    this.Dispatch(OnSpawn);
                }
            }
        }

        public string Name => MHRSunbreakDemoContext.Strings.GetMonsterNameById(Id);

        public float Health
        {
            get => _health;
            private set
            {
                if (_health != value)
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

        public IMonsterPart[] Parts => parts.Values.ToArray();
        public IMonsterAilment[] Ailments => ailments.Values.ToArray();

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
        public event EventHandler<EventArgs> OnEnrageStateChange;
        public event EventHandler<EventArgs> OnTargetChange;
        public event EventHandler<IMonsterPart> OnNewPartFound;
        public event EventHandler<IMonsterAilment> OnNewAilmentFound;

        public MHRSunbreakDemoMonster(IProcessManager process, long address) : base(process)
        {
            _address = address;

            Log.Debug($"Initialized monster at address {address:X}");
        }

        [ScannableMethod(typeof(MonsterInformationData))]
        private void GetMonsterBasicInformation()
        {
            MonsterInformationData dto = new();

            int monsterId = _process.Memory.Read<int>(_address + 0x2B4);

            dto.Id = monsterId;

            Next(ref dto);
            
            Id = dto.Id;
        }

        [ScannableMethod(typeof(HealthData))]
        private void GetMonsterHealthData()
        {
            HealthData dto = new();

            long healthComponent = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_OFFSETS"));
            long healthPtr = _process.Memory.ReadPtr(healthComponent, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

            float currentHealth = _process.Memory.Read<float>(healthPtr + 0x18);

            dto.Health = currentHealth;
            dto.MaxHealth = _process.Memory.Read<float>(healthComponent + 0x18);

            Next(ref dto);

            MaxHealth = dto.MaxHealth;
            Health = dto.Health;
        }

        [ScannableMethod]
        private void ScanLockon()
        {

            int cameraStyleType = _process.Memory.Deref<int>(
                AddressMap.GetAbsolute("LOCKON_ADDRESS"),
                AddressMap.Get<int[]>("LOCKON_CAMERA_STYLE_OFFSETS")
            );

            long cameraStylePtr = _process.Memory.Read(
                AddressMap.GetAbsolute("LOCKON_ADDRESS"),
                AddressMap.Get<int[]>("LOCKON_OFFSETS")
            );

            cameraStylePtr += cameraStyleType * 8;

            long monsterAddress = _process.Memory.Deref<long>(
                    cameraStylePtr,
                    new[] { 0x78 }
            );

            IsTarget = monsterAddress == _address;

            if (IsTarget)
                Target = Target.Self;
            else if (monsterAddress != 0)
                Target = Target.Another;
            else
                Target = Target.None;
        }
    }
}
