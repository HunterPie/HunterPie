using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.DTO.Monster;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Logger;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Runtime.CompilerServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;

#nullable enable
public class MHRMonster : CommonMonster
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
    private readonly object _syncParts = new();
    private readonly Dictionary<long, MHRMonsterAilment> _ailments = new();
    private readonly object _syncAilments = new();
    private readonly List<Element> _weaknesses = new();

    public override int Id
    {
        get => _id;
        protected set
        {
            if (_id != value)
            {
                _id = value;
                GetMonsterWeaknesses();
                this.Dispatch(_onSpawn);
            }
        }
    }

    public override string Name => MHRContext.Strings.GetMonsterNameById(Id);

    public override float Health
    {
        get => _health;
        protected set
        {
            if (_health != value)
            {
                _health = value;
                this.Dispatch(_onHealthChange);

                if (Health <= 0)
                    this.Dispatch(_onDeath);
            }
        }
    }

    public override float MaxHealth { get; protected set; }

    public override float Stamina
    {
        get => _stamina;
        protected set
        {
            if (value != _stamina)
            {
                _stamina = value;
                this.Dispatch(_onStaminaChange);
            }
        }
    }

    public override float MaxStamina { get; protected set; }

    public override bool IsTarget
    {
        get => _isTarget;
        protected set
        {
            if (_isTarget != value)
            {
                _isTarget = value;
                this.Dispatch(_onTarget);
            }
        }
    }

    public override Target Target
    {
        get => _target;
        protected set
        {
            if (_target != value)
            {
                _target = value;
                this.Dispatch(_onTargetChange);
            }
        }
    }

    public override Crown Crown
    {
        get => _crown;
        protected set
        {
            if (_crown != value)
            {
                _crown = value;
                this.Dispatch(_onCrownChange);
            }
        }
    }

    public override IMonsterPart[] Parts
    {
        get
        {
            lock (_syncParts)
            {
                var extraParts = new List<IMonsterPart>();

                if (_qurioThreshold is not null)
                    extraParts.Add(_qurioThreshold);

                extraParts.AddRange(_parts.Values);
                return extraParts.ToArray();
            }
        }
    }

    public override IMonsterAilment[] Ailments
    {
        get
        {
            lock (_syncAilments)
                return _ailments.Values.ToArray();
        }
    }

    public override bool IsEnraged
    {
        get => _isEnraged;
        protected set
        {
            if (value != _isEnraged)
            {
                _isEnraged = value;
                this.Dispatch(_onEnrageStateChange);
            }
        }
    }

    public override IMonsterAilment Enrage => _enrage;

    public override Element[] Weaknesses => _weaknesses.ToArray();

    public override float CaptureThreshold
    {
        get => _captureThreshold;
        protected set
        {
            if (value != _captureThreshold)
            {
                _captureThreshold = value;
                this.Dispatch(_onCaptureThresholdChange, this);
            }
        }
    }

    public MonsterType MonsterType { get; private set; }
    public bool IsQurioActive { get; private set; }

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
            this.Dispatch(_onWeaknessesChange, Weaknesses);
        }
    }

    private void GetMonsterType()
    {
        long monsterTypePtr = Process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_TYPE_OFFSETS")
        );
        int monsterType = Process.Memory.Read<int>(monsterTypePtr + 0x5C);
        MonsterType = (MonsterType)monsterType;
    }

    [ScannableMethod(typeof(MonsterInformationData))]
    private void GetMonsterBasicInformation()
    {
        MonsterInformationData dto = new();

        int monsterId = Process.Memory.Read<int>(_address + 0x2D4);

        dto.Id = monsterId;

        Next(ref dto);

        GetMonsterType();

        Id = dto.Id;
    }

    [ScannableMethod(typeof(HealthData))]
    private void GetMonsterHealthData()
    {
        HealthData dto = new();

        long healthComponent = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_OFFSETS"));
        long encodedPtr = Process.Memory.ReadPtr(healthComponent, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

        uint healthEncodedMod = Process.Memory.Read<uint>(encodedPtr + 0x18) & 3;
        uint healthEncoded = Process.Memory.Read<uint>(encodedPtr + (healthEncodedMod * 4) + 0x1C);
        uint healthEncodedKey = Process.Memory.Read<uint>(encodedPtr + 0x14);

        float currentHealth = MHRCrypto.DecodeHealth(healthEncoded, healthEncodedKey);

        dto.Health = currentHealth;
        dto.MaxHealth = Process.Memory.Read<float>(healthComponent + 0x18);

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

        long captureHealthPtr = Process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_CAPTURE_HEALTH_THRESHOLD")
        );
        float captureHealth = Process.Memory.Read<float>(captureHealthPtr + 0x1C);

        CaptureThreshold = captureHealth / MaxHealth;
    }

    [ScannableMethod]
    private void GetMonsterParts()
    {
        long qurioDataPtr = Process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_QURIO_DATA")
        );
        bool isQurioActive = Process.Memory.Read<byte>(qurioDataPtr + 0x12) == 2;

        // Flinch array
        long monsterFlinchPartsPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_FLINCH_HEALTH_COMPONENT_OFFSETS"));
        uint monsterFlinchPartsArrayLength = Process.Memory.Read<uint>(monsterFlinchPartsPtr + 0x1C);

        // Breakable array
        long monsterBreakPartsArrayPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_BREAK_HEALTH_COMPONENT_OFFSETS"));
        uint monsterBreakPartsArrayLength = Process.Memory.Read<uint>(monsterBreakPartsArrayPtr + 0x1C);

        // Severable array
        long monsterSeverPartsArrayPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_SEVER_HEALTH_COMPONENT_OFFSETS"));
        uint monsterSeverPartsArrayLength = Process.Memory.Read<uint>(monsterSeverPartsArrayPtr + 0x1C);

        // Qurio array
        long monsterQurioPartsArrayPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_QURIO_HEALTH_COMPONENT_OFFSETS"));
        uint monsterQurioPartsArrayLength = Process.Memory.Read<uint>(monsterQurioPartsArrayPtr + 0x1C);

        if (monsterFlinchPartsArrayLength == monsterBreakPartsArrayLength
            && monsterFlinchPartsArrayLength == monsterSeverPartsArrayLength)
        {
            long[] monsterFlinchArray = Process.Memory.Read<long>(monsterFlinchPartsPtr + 0x20, monsterFlinchPartsArrayLength);
            long[] monsterBreakArray = Process.Memory.Read<long>(monsterBreakPartsArrayPtr + 0x20, monsterBreakPartsArrayLength);
            long[] monsterSeverArray = Process.Memory.Read<long>(monsterSeverPartsArrayPtr + 0x20, monsterSeverPartsArrayLength);
            long[] monsterQurioArray = isQurioActive ? Process.Memory.Read<long>(monsterQurioPartsArrayPtr + 0x20, monsterQurioPartsArrayLength) : Array.Empty<long>();

            DerefPartsAndScan(monsterFlinchArray, monsterBreakArray, monsterSeverArray, monsterQurioArray);
        }

        IsQurioActive = isQurioActive;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DerefPartsAndScan(
        IReadOnlyList<long> flinchPointers,
        IReadOnlyList<long> partsPointers,
        IReadOnlyList<long> severablePointers,
        IReadOnlyList<long> qurioPointers
    )
    {
        for (int i = 0; i < flinchPointers.Count; i++)
        {

            long flinchPart = flinchPointers[i];
            long breakablePart = partsPointers[i];
            long severablePart = severablePointers[i];
            long? qurioPart = qurioPointers.Count > 0 ? qurioPointers[i] : null;

            MHRPartStructure partInfo = new()
            {
                MaxHealth = Process.Memory.Read<float>(breakablePart + 0x18),
                MaxFlinch = Process.Memory.Read<float>(flinchPart + 0x18),
                MaxSever = Process.Memory.Read<float>(severablePart + 0x18)
            };

            MHRQurioPartData qurioInfo = new();

            // Skip invalid parts
            if (partInfo.MaxFlinch < 0 && partInfo.MaxHealth < 0 && partInfo.MaxSever < 0)
                continue;

            long encodedFlinchHealthPtr = Process.Memory.ReadPtr(flinchPart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));
            long encodedBreakableHealthPtr = Process.Memory.ReadPtr(breakablePart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));
            long encodedSeverableHealthPtr = Process.Memory.ReadPtr(severablePart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

            if (qurioPart is { } qurioPartPtr)
            {
                MHRQurioPartStructure qurioStructure = Process.Memory.Read<MHRQurioPartStructure>(qurioPartPtr);

                long encryptedHealthPtr =
                    Process.Memory.ReadPtr(qurioStructure.EncryptedHealthPtr, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

                qurioInfo = new MHRQurioPartData
                {
                    MaxHealth = qurioStructure.MaxHealth,
                    Health = Process.Memory.ReadEncryptedFloat(encryptedHealthPtr),
                    IsInQurioState = qurioStructure.IsActive
                };
            }

            partInfo.Flinch = Process.Memory.ReadEncryptedFloat(encodedFlinchHealthPtr);
            partInfo.Health = Process.Memory.ReadEncryptedFloat(encodedBreakableHealthPtr);
            partInfo.Sever = Process.Memory.ReadEncryptedFloat(encodedSeverableHealthPtr);

            lock (_syncParts)
            {
                if (!_parts.ContainsKey(flinchPart))
                {
                    string partName = MonsterData.GetMonsterPartData(Id, i)?.String ?? "PART_UNKNOWN";
                    var dummy = new MHRMonsterPart(partName, partInfo);
                    _parts.Add(flinchPart, dummy);

                    Log.Debug($"Found {partName} for {Name} -> Flinch: {flinchPart:X} Break: {breakablePart:X} Sever: {severablePart:X} Qurio: {qurioPart:X}");
                    this.Dispatch(_onNewPartFound, dummy);
                }

                _parts[flinchPart].Update(partInfo);
                _parts[flinchPart].Update(qurioInfo);
            }
        }
    }

    [ScannableMethod]
    private void GetQurioThreshold()
    {
        if (_qurioThreshold is null)
            return;

        long qurioDataPtr = Process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_QURIO_DATA")
        );
        MHRQurioThresholdStructure structure = Process.Memory.Read<MHRQurioThresholdStructure>(qurioDataPtr + 0x14);
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
        long ailmentsArrayPtr = Process.Memory.ReadPtr(
            _address,
            AddressMap.Get<int[]>("MONSTER_AILMENTS_OFFSETS")
        );

        if (ailmentsArrayPtr.IsNullPointer())
            return;

        int ailmentsArrayLength = Process.Memory.Read<int>(ailmentsArrayPtr + 0x1C);
        long[] ailmentsArray = Process.Memory.Read<long>(ailmentsArrayPtr + 0x20, (uint)ailmentsArrayLength);

        DerefAilmentsAndScan(ailmentsArray);
    }

    [ScannableMethod]
    private void GetLockon()
    {

        int cameraStyleType = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("LOCKON_ADDRESS"),
            AddressMap.Get<int[]>("LOCKON_CAMERA_STYLE_OFFSETS")
        );

        long cameraStylePtr = Process.Memory.Read(
            AddressMap.GetAbsolute("LOCKON_ADDRESS"),
            AddressMap.Get<int[]>("LOCKON_OFFSETS")
        );

        cameraStylePtr += cameraStyleType * 8;

        long monsterAddress = Process.Memory.Deref<long>(
                cameraStylePtr,
                new[] { 0x78 }
        );

        IsTarget = monsterAddress == _address;

        Target = IsTarget ? Target.Self : monsterAddress != 0 ? Target.Another : Target.None;
    }

    [ScannableMethod]
    private void GetMonsterCrown()
    {
        long monsterSizePtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_CROWN_OFFSETS"));
        MHRSizeStructure monsterSize = Process.Memory.Read<MHRSizeStructure>(monsterSizePtr + 0x24);
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
        long enragePtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_ENRAGE_OFFSETS"));

        MHREnrageStructure structure = Process.Memory.Read<MHREnrageStructure>(enragePtr);

        structure.Timer = structure.Timer > 0 ? structure.MaxTimer - structure.Timer : 0;

        _enrage.Update(structure);

        IsEnraged = structure.Timer > 0;
    }

    [ScannableMethod(typeof(MHRStaminaStructure))]
    private void GetMonsterStamina()
    {
        long staminaPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_STAMINA_OFFSETS"));

        MHRStaminaStructure structure = Process.Memory.Read<MHRStaminaStructure>(staminaPtr);

        MaxStamina = structure.MaxStamina;
        Stamina = structure.Stamina;
    }

    private void DerefAilmentsAndScan(long[] ailmentsPointers)
    {
        lock (_syncAilments)
        {
            int ailmentId = 0;
            foreach (long ailmentAddress in ailmentsPointers)
            {
                MHRMonsterAilmentStructure structure = Process.Memory.Read<MHRMonsterAilmentStructure>(ailmentAddress);

                int counter = Process.Memory.Read<int>(structure.CounterPtr + 0x20);
                float buildup = Process.Memory.Read<float>(structure.BuildUpPtr + 0x20);
                float maxBuildup = Process.Memory.Read<float>(structure.MaxBuildUpPtr + 0x20);

                if (!_ailments.ContainsKey(ailmentAddress))
                {
                    MHRMonsterAilment dummy = new(MonsterData.GetAilmentData(ailmentId).String);
                    _ailments.Add(ailmentAddress, dummy);

                    this.Dispatch(_onNewAilmentFound, dummy);
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

    public override void Dispose()
    {
        lock (_syncAilments)
        {
            _ailments.Values.DisposeAll();

            _ailments.Clear();

            _enrage.Dispose();
        }

        lock (_syncParts)
        {
            _qurioThreshold?.Dispose();

            _parts.Values.DisposeAll();

            _parts.Clear();
        }

        base.Dispose();
    }
}
