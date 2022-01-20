using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.DTO.Monster;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Crypto;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRMonster : Scannable, IMonster, IEventDispatcher
    {
        private long _address;

        private int _id = -1;
        private float _health;
        private bool _isTarget;
        private Target _target;
        private readonly Dictionary<long, MHRMonsterPart> parts = new();

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

        public IMonsterPart[] Parts => parts.Values.ToArray();

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
        public event EventHandler<EventArgs> OnTargetChange;

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

            long monsterPartsPtr = _process.Memory.Read<long>(healthComponent + 0x70);
            uint monsterPartsArrayLength = _process.Memory.Read<uint>(monsterPartsPtr + 0x1C);

            // TODO: Find a better way to detect if this array is valid
            // This is a workaround because I don't want HunterPie to allocate 278326178362 longs because it read an invalid value
            if (monsterPartsArrayLength <= 30)
            {
                long[] monsterPartsArray = _process.Memory.Read<long>(monsterPartsPtr, monsterPartsArrayLength);

                DerefPartsAndScan(monsterPartsArray);
            }


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

            bool useFocusCamera = _process.Memory.Read<byte>(address + 0x7A) == 1;

            long monsterAddress = useFocusCamera switch
            {
                true => _process.Memory.Read<long>(address + 0xC0),
                false => _process.Memory.Read<long>(address),
            };

            IsTarget = monsterAddress == _address;
            
            if (IsTarget)
                Target = Target.Self;
            else if (monsterAddress != 0)
                Target = Target.Another;
            else
                Target = Target.None;
        }

        private void DerefPartsAndScan(long[] partsPointers)
        {
            foreach (long part in partsPointers)
            {
                float maxHealth = _process.Memory.Read<float>(part + 0x18);
                // TODO: Read all this in 1 pass
                long encodedHealthPtr = _process.Memory.ReadPtr(part, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));
                uint healthEncodedIdx = _process.Memory.Read<uint>(encodedHealthPtr + 0x18) & 3;
                uint healthEncoded = _process.Memory.Read<uint>(encodedHealthPtr + healthEncodedIdx * 4 + 0x1C);
                uint healthEncodedKey = _process.Memory.Read<uint>(encodedHealthPtr + 0x14);

                float health = MHRFloat.DecodeHealth(healthEncoded, healthEncodedKey);

                if (maxHealth <= 0.0f)
                    continue;

                if (!parts.ContainsKey(part))
                    parts.Add(part, new MHRMonsterPart());

                MHRMonsterPart monsterPart = parts[part];
                monsterPart.UpdateHealth(health, maxHealth);
            }
        }
    }
}
