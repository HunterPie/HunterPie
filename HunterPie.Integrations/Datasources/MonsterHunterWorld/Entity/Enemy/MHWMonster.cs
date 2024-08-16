﻿using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Logger;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enemy;

public sealed class MHWMonster : CommonMonster
{
    #region Private
    private readonly MonsterDefinition _definition;
    private readonly long _address;
    private bool _isLoaded;
    private int _doubleLinkedListIndex;
    private float _health = -1;
    private bool _isEnraged;
    private Target _target;
    private Target _manualTarget;
    private Crown _crown;
    private float _stamina;
    private float _captureThreshold;
    private readonly MHWMonsterAilment _enrage = new(MonsterAilmentRepository.Enrage);
    private readonly (long, MHWMonsterPart)[] _parts;
    private List<(long, MHWMonsterAilment)>? _ailments;
    #endregion

    public override int Id { get; protected set; }

    public string Em { get; }

    public override string Name => MHWContext.Strings.GetMonsterNameById(Id);

    public override float Health
    {
        get => _health;
        protected set
        {
            if (value != _health)
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

    public override IMonsterPart[] Parts => _parts.Where(it => it.Item1 != 0)
                                                .Select(it => it.Item2)
                                                .ToArray<IMonsterPart>();

    public override IMonsterAilment[] Ailments => _ailments?
                                          .Select(a => a.Item2)
                                          .ToArray<IMonsterAilment>() ?? Array.Empty<IMonsterAilment>();

    public override Target Target
    {
        get => _target;
        protected set
        {
            if (_target == value)
                return;

            _target = value;
            this.Dispatch(_onTargetChange, new MonsterTargetEventArgs(this));
        }
    }

    public override Target ManualTarget
    {
        get => _manualTarget;
        protected set
        {
            if (_manualTarget == value)
                return;

            _manualTarget = value;
            this.Dispatch(_onTargetChange, new MonsterTargetEventArgs(this));
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

    private readonly List<Element> _weaknesses = new();

    public override Element[] Weaknesses => _weaknesses.ToArray();

    public override string[] Types { get; } = Array.Empty<string>();

    private bool _isCaptured;
    public bool IsCaptured
    {
        get => _isCaptured;
        private set
        {
            if (value == _isCaptured)
                return;

            _isCaptured = value;
            this.Dispatch(_onCapture, EventArgs.Empty);
        }
    }

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

    public MHWMonster(IProcessManager process, long address, string em) : base(process)
    {
        _address = address;
        Em = em;
        Id = Memory.Read<int>(_address + 0x12280);

        _definition = MonsterRepository.FindBy(GameType.World, Id) ?? MonsterRepository.UnknownDefinition;
        _parts = _definition.Parts.Select(it => (0L, new MHWMonsterPart(it)))
            .ToArray();
        UpdateDetails();
    }

    private void UpdateDetails()
    {
        CaptureThreshold = _definition.Capture / 100f;
        _weaknesses.AddRange(_definition.Weaknesses);
        this.Dispatch(_onWeaknessesChange, Weaknesses);
    }

    [ScannableMethod]
    private void GetMonsterBasicInformation()
    {
        int doubleLinkedListIndex = Process.Memory.Read<int>(_address + 0x1228C);

        _doubleLinkedListIndex = doubleLinkedListIndex;
    }

    [ScannableMethod]
    private void GetMonsterHealthData()
    {
        long monsterHealthPtr = Process.Memory.Read<long>(_address + 0x7670);
        float[] healthValues = Process.Memory.Read<float>(monsterHealthPtr + 0x60, 2);

        MaxHealth = healthValues[0];
        Health = healthValues[1];
    }

    [ScannableMethod]
    private void GetMonsterStaminaData()
    {
        float[] staminaValues = Process.Memory.Read<float>(_address + 0x1C0F0, 2);

        MaxStamina = staminaValues[1];
        Stamina = staminaValues[0];
    }

    [ScannableMethod]
    private void GetMonsterCrownData()
    {
        float sizeModifier = Process.Memory.Read<float>(_address + 0x7730);
        float sizeMultiplier = Process.Memory.Read<float>(_address + 0x184);

        if (sizeModifier is <= 0 or >= 2)
            sizeModifier = 1;

        float monsterSizeMultiplier = (float)Math.Round(sizeMultiplier / sizeModifier * 100f) / 100f;

        MonsterSizeDefinition? crownData = MonsterData.GetMonsterData(Id)?.Size;

        if (crownData is null)
            return;

        MonsterSizeDefinition crown = crownData.Value;

        Crown = monsterSizeMultiplier >= crown.Gold ? Crown.Gold
            : monsterSizeMultiplier >= crown.Silver ? Crown.Silver
                : monsterSizeMultiplier <= crown.Mini ? Crown.Mini
                    : Crown.None;
    }

    [ScannableMethod]
    private void GetAction()
    {
        long actionPtr = _address + 0x61C8;
        int actionId = Memory.Read<int>(actionPtr + 0xB0);
        actionPtr = Memory.ReadPtr(actionPtr, new[] { (2 * 8) + 0x68, actionId * 8, 0, 0x20 });
        uint actionOffset = Memory.Read<uint>(actionPtr + 3);
        long actionRef = Memory.Read<long>(actionPtr + actionOffset + 7 + 8);
        string? action = Memory.Read(actionRef, 64).SanitizeActionString();

        if (action is null)
            return;

        IsCaptured = action.Contains("Capture");
    }

    [ScannableMethod]
    private void GetMonsterEnrage()
    {
        MHWMonsterStatusStructure enrageStructure = Process.Memory.Read<MHWMonsterStatusStructure>(_address + 0x1BE30);
        IUpdatable<MHWMonsterStatusStructure> enrage = _enrage;

        IsEnraged = enrageStructure.Duration > 0;

        enrage.Update(enrageStructure);
    }

    [ScannableMethod]
    private void GetLockedOnMonster()
    {
        long lockedOnDoubleLinkedListAddress = Memory.Read(
            AddressMap.GetAbsolute("LOCKON_ADDRESS"),
            AddressMap.Get<int[]>("LOCKEDON_MONSTER_INDEX_OFFSETS")
        );

        if (lockedOnDoubleLinkedListAddress.IsNullPointer())
        {
            Target = Target.None;
            return;
        }

        int lockedOnDoubleLinkedListIndex = Memory.Read<int>(lockedOnDoubleLinkedListAddress + 0x950);

        bool isTarget = lockedOnDoubleLinkedListIndex == _doubleLinkedListIndex;

        if (isTarget)
            Target = Target.Self;
        else if (lockedOnDoubleLinkedListIndex < 0)
            Target = Target.None;
        else
            Target = Target.Another;
    }

    [ScannableMethod]
    private void GetManualTargetedMonster()
    {
        long questTargetAddress = Memory.Deref<long>(
            AddressMap.GetAbsolute("MONSTER_QUEST_TARGET_ADDRESS"),
            AddressMap.GetOffsets("MONSTER_QUEST_TARGET_OFFSETS")
        );
        MHWMapMonsterSelectionStructure mapSelection = Memory.Deref<MHWMapMonsterSelectionStructure>(
            AddressMap.GetAbsolute("MONSTER_MANUAL_TARGET_ADDRESS"),
            AddressMap.GetOffsets("MONSTER_MANUAL_TARGET_OFFSETS")
        );
        bool isTargetByQuest = !questTargetAddress.IsNullPointer();
        bool isTargetPinned = !mapSelection.MapInsectsRef.IsNullPointer() && !mapSelection.GuiRadarRef.IsNullPointer();
        bool isSelected = isTargetByQuest ? questTargetAddress == _address : mapSelection.SelectedMonster == _address;

        ManualTarget = (isTargetPinned || isTargetByQuest) switch
        {
            true when isSelected => Target.Self,
            true => Target.Another,
            _ => Target.None
        };
    }

    [ScannableMethod]
    private void GetMonsterParts()
    {
        long monsterPartPtr = Process.Memory.Read<long>(_address + 0x1D058);

        if (monsterPartPtr == 0)
            return;

        long monsterPartAddress = monsterPartPtr + 0x40;
        long monsterSeverableAddress = monsterPartPtr + 0x1FC8;

        int normalPartIndex = 0;

        for (int pIndex = 0; pIndex < _parts.Length; pIndex++)
        {
            (long cachedAddress, MHWMonsterPart part) = _parts[pIndex];
            MonsterPartDefinition partSchema = part.Definition;
            MHWMonsterPartStructure partStructure = new();

            // If the part address has been cached already, we can just read them
            if (cachedAddress > 0)
            {
                partStructure = Process.Memory.Read<MHWMonsterPartStructure>(cachedAddress);

                // Alatreon elemental explosion level
                if (Id == 87 && partStructure.Index == 3)
                    partStructure.Counter = Process.Memory.Read<int>(_address + 0x20920);

                part.Update(partStructure);
                continue;
            }

            if (partSchema.IsSeverable)
                while (monsterSeverableAddress < (monsterSeverableAddress + (0x120 * 32)))
                {
                    if (Process.Memory.Read<int>(monsterSeverableAddress) <= 0xA0)
                        monsterSeverableAddress += 0x8;

                    partStructure = Process.Memory.Read<MHWMonsterPartStructure>(monsterSeverableAddress);

                    if (partStructure.Index == partSchema.Id && partStructure.MaxHealth > 0)
                    {
                        _parts[pIndex] = (monsterSeverableAddress, part);

                        this.Dispatch(_onNewPartFound, part);

                        do
                            monsterSeverableAddress += 0x78;
                        while (partStructure.Equals(Process.Memory.Read<MHWMonsterPartStructure>(monsterSeverableAddress)));

                        break;
                    }

                    monsterSeverableAddress += 0x78;
                }
            else
            {
                long address = monsterPartAddress + (normalPartIndex * 0x1F8);
                partStructure = Process.Memory.Read<MHWMonsterPartStructure>(address);


                _parts[pIndex] = (address, part);

                this.Dispatch(_onNewPartFound, part);

                normalPartIndex++;
            }

            part.Update(partStructure);
        }
    }

    [ScannableMethod]
    private void GetMonsterPartTenderizes()
    {
        MHWTenderizeInfoStructure[] tenderizeInfos = Process.Memory.Read<MHWTenderizeInfoStructure>(
            _address + 0x1C458,
            10
        );

        foreach (MHWTenderizeInfoStructure tenderizeInfo in tenderizeInfos)
        {
            if (tenderizeInfo.PartId == uint.MaxValue)
                continue;

            MHWMonsterPart[] parts = _parts.Select(p => p.Item2)
                              .Where(p => p.HasTenderizeId(tenderizeInfo.PartId))
                              .ToArray();

            foreach (MHWMonsterPart part in parts)
                part.Update(tenderizeInfo);
        }
    }

    [ScannableMethod]
    private void GetMonsterAilments()
    {
        if (_ailments is null)
        {
            _ailments = new(32);
            long monsterAilmentArrayElement = _address + 0x1BC40;
            long monsterAilmentPtr = Process.Memory.Read<long>(monsterAilmentArrayElement);

            while (monsterAilmentPtr > 1)
            {
                long currentMonsterAilmentPtr = monsterAilmentPtr;
                // Comment from V1 so I don't forget: There's a gap between the monsterAilmentPtr and the actual ailment data
                MHWMonsterAilmentStructure structure = Process.Memory.Read<MHWMonsterAilmentStructure>(currentMonsterAilmentPtr + 0x148);

                monsterAilmentArrayElement += sizeof(long);
                monsterAilmentPtr = Process.Memory.Read<long>(monsterAilmentArrayElement);

                if (structure.Owner != _address)
                    break;

                AilmentDefinition? ailmentDef = MonsterAilmentRepository.FindBy(GameType.World, structure.Id);
                if (ailmentDef is not { } definition || definition.IsUnknown)
                    continue;

                var ailment = new MHWMonsterAilment(definition);

                _ailments.Add((currentMonsterAilmentPtr, ailment));
                this.Dispatch(_onNewAilmentFound, ailment);

                IUpdatable<MHWMonsterAilmentStructure> updatable = ailment;
                updatable.Update(structure);
            }

            return;
        }

        foreach ((long, MHWMonsterAilment) value in _ailments)
        {
            (long address, MHWMonsterAilment ailment) = value;

            MHWMonsterAilmentStructure structure = Process.Memory.Read<MHWMonsterAilmentStructure>(address + 0x148);
            ailment.Update(structure);
        }
    }

    [ScannableMethod]
    private void FinishScan()
    {
        if (_isLoaded)
            return;

        Log.Debug($"Initialized {Name} at address {_address:X} with id: {Id}");
        _isLoaded = true;
        this.Dispatch(_onSpawn, EventArgs.Empty);
    }

    public override void Dispose()
    {
        _enrage.Dispose();

        _parts.Select(it => it.Item2)
            .DisposeAll();

        _ailments?.Select(it => it.Item2)
            .DisposeAll();

        base.Dispose();
    }
}
