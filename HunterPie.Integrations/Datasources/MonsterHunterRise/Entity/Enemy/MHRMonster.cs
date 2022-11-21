using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.DTO.Monster;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Logger;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Runtime.CompilerServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;

#nullable enable
public class MHRMonster : Scannable, IMonster, IEventDispatcher
{
    private readonly long _address;

    private int _id = -1;
    private float _health = -1;
    private bool _isTarget;
    private bool _isEnraged;
    private Target _target;
    private Crown _crown;
    private float _stamina;
    private float _captureThreshold;
    private readonly MHRMonsterAilment _enrage = new("STATUS_ENRAGE");
    private readonly MHRMonsterPart? _qurioThreshold;
    private readonly Dictionary<long, MHRMonsterPart> _parts = new();
    private readonly Dictionary<long, MHRMonsterAilment> _ailments = new();
    private readonly List<Element> _weaknesses = new();

    public int Id
    {
        get => _id;
        private set
        {
            if (_id != value)
            {
                _id = value;
                GetMonsterWeaknesses();
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

    public IMonsterPart[] Parts
    {
        get
        {
            var extraParts = new List<IMonsterPart>();

            if (_qurioThreshold is not null)
                extraParts.Add(_qurioThreshold);

            extraParts.AddRange(_parts.Values);
            return extraParts.ToArray();
        }
    }

    public IMonsterAilment[] Ailments => _ailments.Values.ToArray();

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

    public Element[] Weaknesses => _weaknesses.ToArray();

    public float CaptureThreshold
    {
        get => _captureThreshold;
        private set
        {
            if (value != _captureThreshold)
            {
                _captureThreshold = value;
                this.Dispatch(OnCaptureThresholdChange, this);
            }
        }
    }

    public MonsterType MonsterType { get; private set; }
    public bool IsQurioActive { get; private set; }

    public event EventHandler<EventArgs>? OnSpawn;
    public event EventHandler<EventArgs>? OnLoad;
    public event EventHandler<EventArgs>? OnDespawn;
    public event EventHandler<EventArgs>? OnDeath;
    public event EventHandler<EventArgs>? OnCapture;
    public event EventHandler<EventArgs>? OnTarget;
    public event EventHandler<EventArgs>? OnCrownChange;
    public event EventHandler<EventArgs>? OnHealthChange;
    public event EventHandler<EventArgs>? OnStaminaChange;
    public event EventHandler<EventArgs>? OnActionChange;
    public event EventHandler<EventArgs>? OnEnrageStateChange;
    public event EventHandler<EventArgs>? OnTargetChange;
    public event EventHandler<IMonsterPart>? OnNewPartFound;
    public event EventHandler<IMonsterAilment>? OnNewAilmentFound;
    public event EventHandler<Element[]>? OnWeaknessesChange;
    public event EventHandler<IMonster>? OnCaptureThresholdChange;

    public MHRMonster(IProcessManager process, long address) : base(process)
    {
        _address = address;
        GetMonsterType();

        if (MonsterType == MonsterType.Qurio)
            _qurioThreshold = new("PART_QURIO_THRESHOLD");

        Log.Debug($"Initialized monster at address {address:X}");
    }

    private void GetMonsterWeaknesses()
    {
        MonsterDataSchema? data = MonsterData.GetMonsterData(Id);

        if (data.HasValue)
        {
            _weaknesses.AddRange(data.Value.Weaknesses);
            this.Dispatch(OnWeaknessesChange, Weaknesses);
        }
    }

    private void GetMonsterType()
    {
        long monsterTypePtr = _process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_TYPE_OFFSETS")
        );
        int monsterType = _process.Memory.Read<int>(monsterTypePtr + 0x5C);
        MonsterType = (MonsterType)monsterType;
    }

    [ScannableMethod(typeof(MonsterInformationData))]
    private void GetMonsterBasicInformation()
    {
        MonsterInformationData dto = new();

        int monsterId = _process.Memory.Read<int>(_address + 0x2D4);

        dto.Id = monsterId;

        Next(ref dto);

        GetMonsterType();

        Id = dto.Id;
    }

    [ScannableMethod(typeof(HealthData))]
    private void GetMonsterHealthData()
    {
        HealthData dto = new();

        long healthComponent = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_OFFSETS"));
        long encodedPtr = _process.Memory.ReadPtr(healthComponent, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

        uint healthEncodedMod = _process.Memory.Read<uint>(encodedPtr + 0x18) & 3;
        uint healthEncoded = _process.Memory.Read<uint>(encodedPtr + (healthEncodedMod * 4) + 0x1C);
        uint healthEncodedKey = _process.Memory.Read<uint>(encodedPtr + 0x14);

        float currentHealth = MHRCrypto.DecodeHealth(healthEncoded, healthEncodedKey);

        dto.Health = currentHealth;
        dto.MaxHealth = _process.Memory.Read<float>(healthComponent + 0x18);

        Next(ref dto);

        MaxHealth = dto.MaxHealth;
        Health = dto.Health;
    }

    [ScannableMethod]
    private void GetMonsterCaptureThreshold()
    {
        if (MonsterType == MonsterType.Qurio)
        {
            CaptureThreshold = 0.0f;
            return;
        }

        MonsterDataSchema? data = MonsterData.GetMonsterData(Id);
        if (data.HasValue && data.Value.IsNotCapturable == true)
        {
            CaptureThreshold = 0.0f;
            return;
        }

        long captureHealthPtr = _process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_CAPTURE_HEALTH_THRESHOLD")
        );
        float captureHealth = _process.Memory.Read<float>(captureHealthPtr + 0x1C);

        CaptureThreshold = captureHealth / MaxHealth;
    }

    [ScannableMethod]
    private void GetMonsterParts()
    {
        long qurioDataPtr = _process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_QURIO_DATA")
        );
        bool isQurioActive = _process.Memory.Read<byte>(qurioDataPtr + 0x12) == 2;

        // Flinch array
        long monsterFlinchPartsPtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_FLINCH_HEALTH_COMPONENT_OFFSETS"));
        uint monsterFlinchPartsArrayLength = _process.Memory.Read<uint>(monsterFlinchPartsPtr + 0x1C);

        // Breakable array
        long monsterBreakPartsArrayPtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_BREAK_HEALTH_COMPONENT_OFFSETS"));
        uint monsterBreakPartsArrayLength = _process.Memory.Read<uint>(monsterBreakPartsArrayPtr + 0x1C);

        // Severable array
        long monsterSeverPartsArrayPtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_SEVER_HEALTH_COMPONENT_OFFSETS"));
        uint monsterSeverPartsArrayLength = _process.Memory.Read<uint>(monsterSeverPartsArrayPtr + 0x1C);

        // Qurio array
        long monsterQurioPartsArrayPtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_QURIO_HEALTH_COMPONENT_OFFSETS"));
        uint monsterQurioPartsArrayLength = _process.Memory.Read<uint>(monsterQurioPartsArrayPtr + 0x1C);

        if (monsterFlinchPartsArrayLength == monsterBreakPartsArrayLength
            && monsterFlinchPartsArrayLength == monsterSeverPartsArrayLength)
        {
            long[] monsterFlinchArray = _process.Memory.Read<long>(monsterFlinchPartsPtr + 0x20, monsterFlinchPartsArrayLength);
            long[] monsterBreakArray = _process.Memory.Read<long>(monsterBreakPartsArrayPtr + 0x20, monsterBreakPartsArrayLength);
            long[] monsterSeverArray = _process.Memory.Read<long>(monsterSeverPartsArrayPtr + 0x20, monsterSeverPartsArrayLength);
            long[] monsterQurioArray = isQurioActive ? _process.Memory.Read<long>(monsterQurioPartsArrayPtr + 0x20, monsterQurioPartsArrayLength) : Array.Empty<long>();

            DerefPartsAndScan(monsterFlinchArray, monsterBreakArray, monsterSeverArray, monsterQurioArray);
        }

        IsQurioActive = isQurioActive;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DerefPartsAndScan(
        long[] flinchPointers,
        long[] partsPointers,
        long[] severablePointers,
        long[] qurioPointers
    )
    {
        for (int i = 0; i < flinchPointers.Length; i++)
        {

            long flinchPart = flinchPointers[i];
            long breakablePart = partsPointers[i];
            long severablePart = severablePointers[i];
            long? qurioPart = qurioPointers.Length > 0 ? qurioPointers[i] : null;

            MHRPartStructure partInfo = new()
            {
                MaxHealth = _process.Memory.Read<float>(breakablePart + 0x18),
                MaxFlinch = _process.Memory.Read<float>(flinchPart + 0x18),
                MaxSever = _process.Memory.Read<float>(severablePart + 0x18)
            };

            MHRQurioPartData qurioInfo = new();

            // Skip invalid parts
            if (partInfo.MaxFlinch < 0 && partInfo.MaxHealth < 0 && partInfo.MaxSever < 0)
                continue;

            long encodedFlinchHealthPtr = _process.Memory.ReadPtr(flinchPart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));
            long encodedBreakableHealthPtr = _process.Memory.ReadPtr(breakablePart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));
            long encodedSeverableHealthPtr = _process.Memory.ReadPtr(severablePart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

            if (qurioPart is { } qurioPartPtr)
            {
                MHRQurioPartStructure qurioStructure = _process.Memory.Read<MHRQurioPartStructure>(qurioPartPtr);

                long encryptedHealthPtr =
                    _process.Memory.ReadPtr(qurioStructure.EncryptedHealthPtr, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

                qurioInfo = new MHRQurioPartData
                {
                    MaxHealth = qurioStructure.MaxHealth,
                    Health = _process.Memory.ReadEncryptedFloat(encryptedHealthPtr),
                    IsInQurioState = qurioStructure.IsActive
                };
            }

            partInfo.Flinch = _process.Memory.ReadEncryptedFloat(encodedFlinchHealthPtr);
            partInfo.Health = _process.Memory.ReadEncryptedFloat(encodedBreakableHealthPtr);
            partInfo.Sever = _process.Memory.ReadEncryptedFloat(encodedSeverableHealthPtr);

            if (!_parts.ContainsKey(flinchPart))
            {
                string partName = MonsterData.GetMonsterPartData(Id, i)?.String ?? "PART_UNKNOWN";
                var dummy = new MHRMonsterPart(partName, partInfo);
                _parts.Add(flinchPart, dummy);

                Log.Debug($"Found {partName} for {Name} -> Flinch: {flinchPart:X} Break: {breakablePart:X} Sever: {severablePart:X} Qurio: {qurioPart:X}");
                this.Dispatch(OnNewPartFound, dummy);
            }

            _parts[flinchPart].Update(partInfo);
            _parts[flinchPart].Update(qurioInfo);
        }
    }

    [ScannableMethod]
    private void GetQurioThreshold()
    {
        if (_qurioThreshold is null)
            return;

        long qurioDataPtr = _process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_QURIO_DATA")
        );
        MHRQurioThresholdStructure structure = _process.Memory.Read<MHRQurioThresholdStructure>(qurioDataPtr + 0x14);
        var data = new MHRQurioPartData
        {
            IsInQurioState = true,
            Health = structure.MaxThreshold - structure.Threshold,
            MaxHealth = structure.MaxThreshold
        };

        _qurioThreshold.Update(data);
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
    private void GetLockon()
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

        Target = IsTarget ? Target.Self : monsterAddress != 0 ? Target.Another : Target.None;
    }

    [ScannableMethod]
    private void GetMonsterCrown()
    {
        long monsterSizePtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_CROWN_OFFSETS"));
        MHRSizeStructure monsterSize = _process.Memory.Read<MHRSizeStructure>(monsterSizePtr + 0x24);
        float monsterSizeMultiplier = monsterSize.SizeMultiplier * monsterSize.UnkMultiplier;

        MonsterSizeSchema? crownData = MonsterData.GetMonsterData(Id)?.Size;

        if (crownData is null)
            return;

        MonsterSizeSchema crown = crownData.Value;

        Crown = monsterSizeMultiplier >= crown.Gold
            ? Crown.Gold
            : monsterSizeMultiplier >= crown.Silver ? Crown.Silver : monsterSizeMultiplier <= crown.Mini ? Crown.Mini : Crown.None;
    }

    [ScannableMethod(typeof(MHREnrageStructure))]
    private void GetMonsterEnrage()
    {
        long enragePtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_ENRAGE_OFFSETS"));

        MHREnrageStructure structure = _process.Memory.Read<MHREnrageStructure>(enragePtr);

        structure.Timer = structure.Timer > 0 ? structure.MaxTimer - structure.Timer : 0;

        _enrage.Update(structure);

        IsEnraged = structure.Timer > 0;
    }

    [ScannableMethod(typeof(MHRStaminaStructure))]
    private void GetMonsterStamina()
    {
        long staminaPtr = _process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_STAMINA_OFFSETS"));

        MHRStaminaStructure structure = _process.Memory.Read<MHRStaminaStructure>(staminaPtr);

        MaxStamina = structure.MaxStamina;
        Stamina = structure.Stamina;
    }

    private void DerefAilmentsAndScan(long[] ailmentsPointers)
    {
        int ailmentId = 0;
        foreach (long ailmentAddress in ailmentsPointers)
        {
            MHRMonsterAilmentStructure structure = _process.Memory.Read<MHRMonsterAilmentStructure>(ailmentAddress);

            int counter = _process.Memory.Read<int>(structure.CounterPtr + 0x20);
            float buildup = _process.Memory.Read<float>(structure.BuildUpPtr + 0x20);
            float maxBuildup = _process.Memory.Read<float>(structure.MaxBuildUpPtr + 0x20);

            if (!_ailments.ContainsKey(ailmentAddress))
            {
                MHRMonsterAilment dummy = new(MonsterData.GetAilmentData(ailmentId).String);
                _ailments.Add(ailmentAddress, dummy);

                this.Dispatch(OnNewAilmentFound, dummy);
                //Log.Debug($"Found new ailment at {ailmentAddress:X08}");
            }

            var data = new MHRAilmentData
            {
                BuildUp = buildup,
                MaxBuildUp = maxBuildup,
                Timer = structure.Timer,
                MaxTimer = structure.MaxTimer,
                Counter = counter,
            };

            MHRMonsterAilment ailment = _ailments[ailmentAddress];
            ailment.Update(data);
            ailmentId++;
        }
    }
}
