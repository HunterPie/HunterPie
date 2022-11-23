using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.DTO.Monster;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Logger;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;
using System.Runtime.CompilerServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo.Entity.Enemy;

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
    private readonly MHRMonsterAilment _enrage = new("STATUS_ENRAGE");
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

                this.Dispatch(_onSpawn);
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
                this.Dispatch(_onHealthChange);

                if (Health <= 0)
                    this.Dispatch(_onDeath);
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
                this.Dispatch(_onStaminaChange);
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
                this.Dispatch(_onTarget);
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
                this.Dispatch(_onTargetChange);
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
                this.Dispatch(_onCrownChange);
            }
        }
    }

    public IMonsterPart[] Parts => _parts.Values.ToArray();
    public IMonsterAilment[] Ailments => _ailments.Values.ToArray();

    public bool IsEnraged
    {
        get => _isEnraged;
        private set
        {
            if (value != _isEnraged)
            {
                _isEnraged = value;
                this.Dispatch(_onEnrageStateChange);
            }
        }
    }

    public IMonsterAilment Enrage => _enrage;

    public Element[] Weaknesses => _weaknesses.ToArray();

    public float CaptureThreshold => 0;

    private readonly SmartEvent<EventArgs> _onSpawn = new();
    public event EventHandler<EventArgs> OnSpawn
    {
        add => _onSpawn.Hook(value);
        remove => _onSpawn.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onLoad = new();
    public event EventHandler<EventArgs> OnLoad
    {
        add => _onLoad.Hook(value);
        remove => _onLoad.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onDespawn = new();
    public event EventHandler<EventArgs> OnDespawn
    {
        add => _onDespawn.Hook(value);
        remove => _onDespawn.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onDeath = new();
    public event EventHandler<EventArgs> OnDeath
    {
        add => _onDeath.Hook(value);
        remove => _onDeath.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onCapture = new();
    public event EventHandler<EventArgs> OnCapture
    {
        add => _onCapture.Hook(value);
        remove => _onCapture.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onTarget = new();
    public event EventHandler<EventArgs> OnTarget
    {
        add => _onTarget.Hook(value);
        remove => _onTarget.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onCrownChange = new();
    public event EventHandler<EventArgs> OnCrownChange
    {
        add => _onCrownChange.Hook(value);
        remove => _onCrownChange.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onHealthChange = new();
    public event EventHandler<EventArgs> OnHealthChange
    {
        add => _onHealthChange.Hook(value);
        remove => _onHealthChange.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onStaminaChange = new();
    public event EventHandler<EventArgs> OnStaminaChange
    {
        add => _onStaminaChange.Hook(value);
        remove => _onStaminaChange.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onActionChange = new();
    public event EventHandler<EventArgs> OnActionChange
    {
        add => _onActionChange.Hook(value);
        remove => _onActionChange.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onEnrageStateChange = new();
    public event EventHandler<EventArgs> OnEnrageStateChange
    {
        add => _onEnrageStateChange.Hook(value);
        remove => _onEnrageStateChange.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onTargetChange = new();
    public event EventHandler<EventArgs> OnTargetChange
    {
        add => _onTargetChange.Hook(value);
        remove => _onTargetChange.Unhook(value);
    }

    private readonly SmartEvent<IMonsterPart> _onNewPartFound = new();
    public event EventHandler<IMonsterPart> OnNewPartFound
    {
        add => _onNewPartFound.Hook(value);
        remove => _onNewPartFound.Unhook(value);
    }

    private readonly SmartEvent<IMonsterAilment> _onNewAilmentFound = new();
    public event EventHandler<IMonsterAilment> OnNewAilmentFound
    {
        add => _onNewAilmentFound.Hook(value);
        remove => _onNewAilmentFound.Unhook(value);
    }

    private readonly SmartEvent<Element[]> _onWeaknessesChange = new();
    public event EventHandler<Element[]> OnWeaknessesChange
    {
        add => _onWeaknessesChange.Hook(value);
        remove => _onWeaknessesChange.Unhook(value);
    }

    private readonly SmartEvent<IMonster> _onCaptureThresholdChange = new();
    public event EventHandler<IMonster> OnCaptureThresholdChange
    {
        add => _onCaptureThresholdChange.Hook(value);
        remove => _onCaptureThresholdChange.Unhook(value);
    }

    public MHRSunbreakDemoMonster(IProcessManager process, long address) : base(process)
    {
        _address = address;

        Log.Debug($"Initialized monster at address {address:X}");
    }

    private void GetMonsterWeaknesses()
    {
        Core.Game.Data.Schemas.MonsterDataSchema? data = MonsterData.GetMonsterData(Id);

        if (data.HasValue)
        {
            _weaknesses.AddRange(data.Value.Weaknesses);
            this.Dispatch(_onWeaknessesChange, Weaknesses);
        }
    }

    [ScannableMethod(typeof(MonsterInformationData))]
    private void GetMonsterBasicInformation()
    {
        MonsterInformationData dto = new();

        int monsterId = Process.Memory.Read<int>(_address + 0x2B4);

        dto.Id = monsterId;

        Next(ref dto);

        Id = dto.Id;
    }

    [ScannableMethod(typeof(HealthData))]
    private void GetMonsterHealthData()
    {
        HealthData dto = new();

        long healthComponent = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_OFFSETS"));
        long healthPtr = Process.Memory.ReadPtr(healthComponent, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

        float currentHealth = Process.Memory.Read<float>(healthPtr + 0x18);

        dto.Health = currentHealth;
        dto.MaxHealth = Process.Memory.Read<float>(healthComponent + 0x18);

        Next(ref dto);

        MaxHealth = dto.MaxHealth;
        Health = dto.Health;
    }

    [ScannableMethod]
    private void ScanLockon()
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

    [ScannableMethod(typeof(MHRStaminaStructure))]
    private void GetMonsterStamina()

    {
        long staminaPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_STAMINA_OFFSETS"));

        MHRStaminaStructure structure = Process.Memory.Read<MHRStaminaStructure>(staminaPtr);

        MaxStamina = structure.MaxStamina;
        Stamina = structure.Stamina;
    }

    [ScannableMethod]
    private void GetMonsterParts()
    {
        // Flinch array
        long monsterFlinchPartsPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_FLINCH_HEALTH_COMPONENT_OFFSETS"));
        uint monsterFlinchPartsArrayLength = Process.Memory.Read<uint>(monsterFlinchPartsPtr + 0x1C);

        // Breakable array
        long monsterBreakPartsArrayPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_BREAK_HEALTH_COMPONENT_OFFSETS"));
        uint monsterBreakPartsArrayLength = Process.Memory.Read<uint>(monsterBreakPartsArrayPtr + 0x1C);

        // Severable array
        long monsterSeverPartsArrayPtr = Process.Memory.ReadPtr(_address, AddressMap.Get<int[]>("MONSTER_SEVER_HEALTH_COMPONENT_OFFSETS"));
        uint monsterSeverPartsArrayLenght = Process.Memory.Read<uint>(monsterSeverPartsArrayPtr + 0x1C);

        if (monsterFlinchPartsArrayLength == monsterBreakPartsArrayLength
            && monsterFlinchPartsArrayLength == monsterSeverPartsArrayLenght)
        {
            long[] monsterFlinchArray = Process.Memory.Read<long>(monsterFlinchPartsPtr + 0x20, monsterFlinchPartsArrayLength);
            long[] monsterBreakArray = Process.Memory.Read<long>(monsterBreakPartsArrayPtr + 0x20, monsterBreakPartsArrayLength);
            long[] monsterSeverArray = Process.Memory.Read<long>(monsterSeverPartsArrayPtr + 0x20, monsterSeverPartsArrayLenght);

            DerefPartsAndScan(monsterFlinchArray, monsterBreakArray, monsterSeverArray);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DerefPartsAndScan(long[] flinchPointers, long[] partsPointers, long[] severablePointers)
    {
        for (int i = 0; i < flinchPointers.Length; i++)
        {

            long flinchPart = flinchPointers[i];
            long breakablePart = partsPointers[i];
            long severablePart = severablePointers[i];

            MHRPartStructure partInfo = new()
            {
                MaxHealth = Process.Memory.Read<float>(breakablePart + 0x18),
                MaxFlinch = Process.Memory.Read<float>(flinchPart + 0x18),
                MaxSever = Process.Memory.Read<float>(severablePart + 0x18)
            };

            // Skip invalid parts
            if (partInfo.MaxFlinch < 0 && partInfo.MaxHealth < 0 && partInfo.MaxSever < 0)
                continue;

            // TODO: Read all this in 1 pass
            long encodedFlinchHealthPtr = Process.Memory.ReadPtr(flinchPart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));
            long encodedBreakableHealthPtr = Process.Memory.ReadPtr(breakablePart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));
            long encodedSeverableHealthPtr = Process.Memory.ReadPtr(severablePart, AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS"));

            // Flinch values
            partInfo.Flinch = Process.Memory.Read<float>(encodedFlinchHealthPtr + 0x18);

            // Break values
            partInfo.Health = Process.Memory.Read<float>(encodedBreakableHealthPtr + 0x18);

            // Sever values
            partInfo.Sever = Process.Memory.Read<float>(encodedSeverableHealthPtr + 0x18);

            if (!_parts.ContainsKey(flinchPart))
            {
                string partName = MonsterData.GetMonsterPartData(Id, i)?.String ?? "PART_UNKNOWN";
                var dummy = new MHRMonsterPart(partName, partInfo);
                _parts.Add(flinchPart, dummy);

                Log.Debug($"Found {partName} for {Name} -> Flinch: {flinchPart:X} Break: {breakablePart:X} Sever: {severablePart:X}");
                this.Dispatch(_onNewPartFound, dummy);
            }

            IUpdatable<MHRPartStructure> monsterPart = _parts[flinchPart];
            monsterPart.Update(partInfo);
        }
    }
}
