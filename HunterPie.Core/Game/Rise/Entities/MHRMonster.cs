using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.DTO.Monster;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Crypto;
using HunterPie.Core.Logger;
using System;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRMonster : Scannable, IMonster, IEventDispatcher
    {
        private long _address;

        private int _id = -1;
        private float _health;
        private bool _isTarget;

        public float Health
        {
            get => _health;
            private set
            {
                if (_health != value)
                {
                    _health = value;
                    this.Dispatch(OnHealthChange);
                }
            }
        }

        public float MaxHealth { get; private set; }

        public string Name => MHRContext.Strings.GetMonsterNameById(Id);

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

        public MHRMonster(IProcessManager process, long address) : base(process)
        {
            _address = address;

            Log.Debug($"Initialized monster at address {address:X}");
        }

        [ScannableMethod(typeof(MonsterInformationData))]
        private void GetMonsterBasicInformation()
        {
            MonsterInformationData dto = new();

            int monsterId = _process.Memory.Read<int>(_address + 0x274);
            
            dto.Id = monsterId;

            Next(ref dto);

            Id = dto.Id;
        }

        [ScannableMethod(typeof(HealthData))]
        private void GetMonsterHealthData()
        {
            HealthData dto = new();
            
            long healthComponent = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_OFFSETS"));
            long encodedPtr = _process.Memory.ReadPtr(healthComponent, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

            uint healthEncodedMod = _process.Memory.Read<uint>(encodedPtr + 0x18) & 3;
            uint healthEncoded = _process.Memory.Read<uint>(encodedPtr + healthEncodedMod * 4 + 0x1C);
            uint healthEncodedKey = _process.Memory.Read<uint>(encodedPtr + 0x14);

            float currentHealth = MHRFloat.DecodeHealth(healthEncoded, healthEncodedKey);

            dto.Health = currentHealth;
            dto.MaxHealth = _process.Memory.Read<float>(healthComponent + 0x18);

            Next(ref dto);

            MaxHealth = dto.MaxHealth;
            Health = dto.Health;
        }

        [ScannableMethod]
        private void ScanLockon()
        {
            long address = _process.Memory.Read(
                    AddressMap.GetAbsolute("LOCKON_ADDRESS"),
                    AddressMap.Get<int[]>("LOCKON_OFFSETS")
            );

            long monsterAddress = _process.Memory.Read<long>(address);

            IsTarget = monsterAddress == _address || monsterAddress == 0;
        }
    }
}
