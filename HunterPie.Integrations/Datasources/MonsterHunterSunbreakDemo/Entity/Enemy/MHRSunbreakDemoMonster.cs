using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Logger;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;
using System.Runtime.CompilerServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo.Entity.Enemy;

public sealed class MHRSunbreakDemoMonster : CommonMonster
{
    private readonly MonsterDefinition _definition;
    private readonly long _address;

    private int _id = -1;
    private float _health = -1;
    private bool _isEnraged;
    private Target _target;
    private Crown _crown;
    private float _stamina;
    private readonly MHRMonsterAilment _enrage = new(MonsterAilmentRepository.Enrage);
    private readonly Dictionary<long, MHRMonsterPart> _parts = new();
    private readonly Dictionary<long, MHRMonsterAilment> _ailments = new();
    private readonly List<Element> _weaknesses = new();

    public override int Id
    {
        get => _id;
        protected set
        {
            if (_id != value)
            {
                _id = value;
                this.Dispatch(_onSpawn);
            }
        }
    }

    public override string Name => MHRSunbreakDemoContext.Strings.GetMonsterNameById(Id);

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

    public override Target Target
    {
        get => _target;
        protected set
        {
            if (_target != value)
            {
                _target = value;
                this.Dispatch(_onTargetChange, new MonsterTargetEventArgs(this));
            }
        }
    }

    public override Target ManualTarget { get; protected set; } = Target.None;

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

    public override IMonsterPart[] Parts => _parts.Values.ToArray();
    public override IMonsterAilment[] Ailments => _ailments.Values.ToArray();

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
    public override string[] Types { get; } = Array.Empty<string>();

    public override float CaptureThreshold { get; protected set; } = 0f;

    public MHRSunbreakDemoMonster(IProcessManager process, long address) : base(process)
    {
        _address = address;

        Id = Process.Memory.Read<int>(_address + 0x2B4);

        _definition = MonsterRepository.FindBy(GameType.Rise, Id) ?? MonsterRepository.UnknownDefinition;

        Log.Debug($"Initialized monster at address {address:X}");
    }

    private void UpdateData()
    {
        _weaknesses.AddRange(_definition.Weaknesses);
        this.Dispatch(_onWeaknessesChange, Weaknesses);
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

        bool isTarget = monsterAddress == _address;

        Target = isTarget ? Target.Self : monsterAddress != 0 ? Target.Another : Target.None;
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
                MonsterPartDefinition definition = _definition.Parts.Length > i
                    ? _definition.Parts[i]
                    : MonsterRepository.UnknownPartDefinition;

                var dummy = new MHRMonsterPart(definition, partInfo);
                _parts.Add(flinchPart, dummy);

                Log.Debug($"Found {definition.String} for {Name} -> Flinch: {flinchPart:X} Break: {breakablePart:X} Sever: {severablePart:X}");
                this.Dispatch(_onNewPartFound, dummy);
            }

            IUpdatable<MHRPartStructure> monsterPart = _parts[flinchPart];
            monsterPart.Update(partInfo);
        }
    }

    public override void Dispose()
    {
        _parts.Values.DisposeAll();
        _ailments.Values.DisposeAll();
        _parts.Clear();
        _ailments.Clear();
        base.Dispose();
    }
}
