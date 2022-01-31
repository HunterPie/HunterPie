
using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.DTO.Monster;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Crypto;
using HunterPie.Core.Game.Rise.Definitions;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.Rise.Entities
{
#pragma warning disable IDE0051 // Remove unused private members
    public class MHRMonster : Scannable, IMonster, IEventDispatcher
    {
        private long _address;

        private int _id = -1;
        private float _health;
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

        public string Name => MHRContext.Strings.GetMonsterNameById(Id);
        
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
        private void GetMonsterAilments()
        {
            long ailmentsArrayPtr = _process.Memory.ReadPtr(
                _address, 
                AddressMap.Get<int[]>("MONSTER_AILMENTS_OFFSETS")
            );

            int ailmentsArrayLength = _process.Memory.Read<int>(ailmentsArrayPtr + 0x1C);
            long[] ailmentsArray = _process.Memory.Read<long>(ailmentsArrayPtr + 0x20, (uint)ailmentsArrayLength);

            DerefAilmentsAndScan(ailmentsArray);
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

        [ScannableMethod]
        private void ScanMonsterCrown()
        {
            long monsterSizePtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_CROWN_OFFSETS"));
            float monsterSizeMultiplier = _process.Memory.Read<float>(monsterSizePtr + 0x28);
            
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

        [ScannableMethod(typeof(MHREnrageStructure))]
        private void ScanMonsterEnrage()
        {
            long enragePtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_ENRAGE_OFFSETS"));
            
            MHREnrageStructure structure = _process.Memory.Read<MHREnrageStructure>(enragePtr);

            _enrage.UpdateInfo(
                // To reverse the timer so it counts down instead of up
                structure.Timer > 0 
                    ? structure.MaxTimer - structure.Timer 
                    : 0,
                structure.MaxTimer,
                structure.BuildUp,
                structure.MaxBuildUp,
                structure.Counter
            );

            IsEnraged = structure.Timer > 0;
        }

        [ScannableMethod(typeof(MHRStaminaStructure))]
        private void ScanMonsterStamina()

        {
            long staminaPtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_STAMINA_OFFSETS"));

            MHRStaminaStructure structure = _process.Memory.Read<MHRStaminaStructure>(staminaPtr);

            MaxStamina = structure.MaxStamina;
            Stamina = structure.Stamina;
        }

        private void DerefPartsAndScan(long[] partsPointers)
        {
            int i = 0;
            foreach (var part in partsPointers)
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
                {
                    var dummy = new MHRMonsterPart(MonsterData.GetMonsterPartData(Id, i)?.String ?? "PART_UNKNOWN");
                    parts.Add(part, dummy);

                    this.Dispatch(OnNewPartFound, dummy);
                }

                MHRMonsterPart monsterPart = parts[part];
                monsterPart.UpdateHealth(health, maxHealth);
                i++;
            }
        }

        private void DerefAilmentsAndScan(long[] ailmentsPointers)
        {
            foreach (long ailmentAddress in ailmentsPointers)
            {
                // TODO: Ailment structure
                // 0x18 Counter Ptr + 20
                // 0x58 Buildup ptr + 20
                // 0x68 Max buildup ptr + 20
                // 0x38 DOT Damage
                // 0x40 MaxTimer
                // 0x44 Timer
                // 0x98 AilmentId

                long counterPtr = _process.Memory.Read<long>(ailmentAddress + 0x18);
                long buildupPtr = _process.Memory.Read<long>(ailmentAddress + 0x58);
                long maxBuildupPtr = _process.Memory.Read<long>(ailmentAddress + 0x68);

                float maxTimer = _process.Memory.Read<float>(ailmentAddress + 0x40);
                float timer = _process.Memory.Read<float>(ailmentAddress + 0x44);
                int ailmentId = _process.Memory.Read<int>(ailmentAddress + 0x98);
                int counter = _process.Memory.Read<int>(counterPtr + 0x20);
                float buildup = _process.Memory.Read<float>(buildupPtr + 0x20);
                float maxBuildup = _process.Memory.Read<float>(maxBuildupPtr + 0x20);

                if (!ailments.ContainsKey(ailmentAddress))
                {
                    MHRMonsterAilment dummy = new(MonsterData.GetAilmentData(ailmentId).String);
                    ailments.Add(ailmentAddress, dummy);

                    this.Dispatch(OnNewAilmentFound, dummy);
                    Log.Debug($"Found new ailment at {ailmentAddress:X08}");
                }

                MHRMonsterAilment ailment = ailments[ailmentAddress];
                ailment.UpdateInfo(timer, maxTimer, buildup, maxBuildup, counter);
            }
        }
    }
#pragma warning restore IDE0051 // Remove unused private members
}
