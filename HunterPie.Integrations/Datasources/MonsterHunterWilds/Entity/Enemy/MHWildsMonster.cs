﻿using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public sealed class MHWildsMonster : CommonMonster
{
    private bool _isDeadOrCaptured = false;
    private readonly MonsterDefinition _definition;
    private readonly MHWildsMonsterBasicData _basicData;
    private bool _isInitialized;
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly MHWildsCryptoService _cryptoService;
    private readonly nint _address;
    private readonly MHWildsMonsterAilment _enrage = new(MonsterAilmentRepository.Enrage);

    private readonly List<MHWildsMonsterAilment> _ailments = new(45);
    private readonly List<MHWildsMonsterPart> _parts = new();

    public override string Name { get; }

    public override int Id { get; protected set; }

    private float _health = -1;
    public override float Health
    {
        get => _health;
        protected set
        {
            if (_health.Equals(value))
                return;

            _health = value;
            this.Dispatch(_onHealthChange);

            if (value <= 0)
            {
                _isDeadOrCaptured = true;
                this.Dispatch(_onDeath);
            }
        }
    }

    public override float MaxHealth { get; protected set; }

    private float _stamina;
    public override float Stamina
    {
        get => _stamina;
        protected set
        {
            if (_stamina.Equals(value))
                return;

            _stamina = value;
            this.Dispatch(_onStaminaChange);
        }
    }

    public override float MaxStamina { get; protected set; }

    private float _captureThreshold;
    public override float CaptureThreshold
    {
        get => _captureThreshold;
        protected set
        {
            if (_captureThreshold.Equals(value))
                return;

            _captureThreshold = value;
            this.Dispatch(_onCaptureThresholdChange, this);
        }
    }

    private bool _isEnrage;
    public override bool IsEnraged
    {
        get => _isEnrage;
        protected set
        {
            if (_isEnrage == value)
                return;

            _isEnrage = value;
            this.Dispatch(
                toDispatch: _onEnrageStateChange,
                data: EventArgs.Empty
            );
        }
    }

    private Target _target;
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

    private Target _manualTarget;
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

    public override IReadOnlyCollection<IMonsterPart> Parts => _parts;

    public override IReadOnlyCollection<IMonsterAilment> Ailments => _ailments;

    public override IMonsterAilment Enrage => _enrage;

    private Crown _crown;
    public override Crown Crown
    {
        get => _crown;
        protected set
        {
            if (_crown == value)
                return;

            _crown = value;
            this.Dispatch(_onCrownChange);
        }
    }

    public override Element[] Weaknesses => _definition.Weaknesses;

    public override string[] Types => _definition.Types;

    public MHWildsMonster(
        IGameProcess process,
        IScanService scanService,
        nint address,
        MHWildsMonsterBasicData basicData,
        MHWildsCryptoService cryptoService,
        ILocalizationRepository localizationRepository) : base(process, scanService)
    {
        _basicData = basicData;
        _address = address;
        Id = basicData.Id;

        string namePath = $"//Strings/Monsters/Wilds/Monster[@Id='{Id}']";

        Name = localizationRepository.ExistsBy(namePath)
            ? localizationRepository.FindStringBy(namePath)
            : $"Unknown [id: {Id}]";
        _cryptoService = cryptoService;
        _definition = MonsterRepository.FindBy(
            game: GameType.Wilds,
            id: Id
        ) ?? MonsterRepository.UnknownDefinition;
    }

    [ScannableMethod]
    internal async Task GetHealthAsync()
    {
        MHWildsMonsterHealth healthComponent = await Memory.DerefPtrAsync<MHWildsMonsterHealth>(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::Health")
        );
        MHWildsEncryptedFloat encryptedHealth = await Memory.ReadAsync<MHWildsEncryptedFloat>(
            address: healthComponent.HealthPointer
        );
        MHWildsEncryptedFloat encryptedMaxHealth = await Memory.ReadAsync<MHWildsEncryptedFloat>(
            address: healthComponent.MaxHealthPointer
        );

        MaxHealth = await _cryptoService.DecryptFloat(encryptedMaxHealth);
        Health = await _cryptoService.DecryptFloat(encryptedHealth);
    }

    [ScannableMethod]
    internal async Task GetStaminaAsync()
    {
        MHWildsAilment ailment = await Memory.DerefPtrAsync<MHWildsAilment>(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::Stamina")
        );


        if (ailment.IsActive == 1)
        {
            MaxStamina = ailment.Timer.Max;
            Stamina = MaxStamina - ailment.Timer.Current;
        }
        else
        {
            MHWildsBuildUp stamina = await Memory.ReadAsync<MHWildsBuildUp>(ailment.BuildUpPointer);

            MaxStamina = stamina.Max;
            Stamina = stamina.Current;
        }
    }

    [ScannableMethod]
    internal async Task GetThresholdsAsync()
    {
        if (_definition is { IsNotCapturable: true })
        {
            CaptureThreshold = 0.0f;
            return;
        }

        MHWildsMonsterThresholds thresholds = await Memory.DerefPtrAsync<MHWildsMonsterThresholds>(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::Thresholds")
        );

        CaptureThreshold = thresholds.Capture;
    }

    [ScannableMethod]
    internal async Task GetAIAsync()
    {
        MHWildsMonsterAI ai = await Memory.DerefPtrAsync<MHWildsMonsterAI>(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::AI")
        );

        if (ai.CurrentActionId == 10 && !_isDeadOrCaptured)
        {
            _logger.Debug($"Captured monster {Name} [{_address:X08}]");
            _isDeadOrCaptured = true;
            this.Dispatch(
                toDispatch: _onCapture,
                data: EventArgs.Empty
            );
        }
    }

    [ScannableMethod]
    internal async Task GetCrownAsync()
    {
        short size;
        // to handle Alpha Doshaguma
        // cmp dword ptr [rdx+48],10
        // jne MonsterHunterWilds.exe+79C635F
        // mov ax,0082
        if (Id == 0x10 && _basicData.RoleId == 1)
            size = 130;
        else
        {
            MHWildsMonsterContext data = await Memory.DerefPtrAsync<MHWildsMonsterContext>(
                address: _address,
                offsets: AddressMap.GetOffsets("Monster::ContextData")
            );

            size = data switch
            {
                { HasFixedSize: true } => data.FixedSize,
                _ => data.Size
            };
        }

        Crown = size >= _definition.Size.Gold
            ? Crown.Gold
            : size >= _definition.Size.Silver
                ? Crown.Silver
                : size <= _definition.Size.Mini
                    ? Crown.Mini
                    : Crown.None;
    }

    [ScannableMethod]
    internal async Task GetEnrageAsync()
    {
        MHWildsAilment ailment = await Memory.DerefPtrAsync<MHWildsAilment>(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::Enrage")
        );

        MHWildsBuildUp buildUp = await Memory.ReadAsync<MHWildsBuildUp>(ailment.BuildUpPointer);

        _enrage.Update(ailment);
        _enrage.Update(buildUp);
        IsEnraged = ailment.Timer.Current > 0;
    }

    [ScannableMethod]
    internal async Task GetTargetAsync()
    {
        nint monsterContextAddress = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::Context")
        );
        nint targetAddress = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("Game::CameraManager"),
            offsets: AddressMap.GetOffsets("Camera::Monster::Target")
        );

        bool isTarget = monsterContextAddress == targetAddress;

        Target = (isTarget, targetAddress) switch
        {
            (true, _) => Target.Self,
            (false, 0) => Target.None,
            (false, _) => Target.Another
        };
    }

    [ScannableMethod]
    internal async Task GetAilmentsAsync()
    {
        nint ailmentsArray = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::Ailments")
        );

        IAsyncEnumerable<MHWildsAilment> ailments = Memory.ReadArrayOfPtrsSafeAsync<MHWildsAilment>(ailmentsArray, 30);

        int index = 0;
        await foreach (MHWildsAilment ailment in ailments)
        {
            bool isNewAilment = index >= _ailments.Count;

            if (isNewAilment)
            {
                AilmentDefinition? definition = MonsterAilmentRepository.FindBy(GameType.Wilds, ailment.Id);

                if (definition is null)
                    continue;

                _ailments.Add(new MHWildsMonsterAilment(definition.Value));
                _logger.Debug($"Found new ailment with id: {ailment.Id} for monster {_address:X08}");
            }

            MHWildsMonsterAilment ailmentEntity = _ailments[index];

            MHWildsBuildUp buildUp = await Memory.ReadAsync<MHWildsBuildUp>(ailment.BuildUpPointer);

            ailmentEntity.Update(ailment);
            ailmentEntity.Update(buildUp);

            if (isNewAilment)
                this.Dispatch(
                    toDispatch: _onNewAilmentFound,
                    data: ailmentEntity
                );
            index++;
        }
    }

    [ScannableMethod]
    internal async Task GetPartsAsync()
    {
        nint partsHealthArray = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::PartsHealth")
        );
        IAsyncEnumerable<MHWildsPartHealth> parts = Memory.ReadArrayOfPtrsAsync<MHWildsPartHealth>(partsHealthArray);

        int index = 0;
        await foreach (MHWildsPartHealth part in parts)
        {
            MHWildsEncryptedFloat encryptedHealth = await Memory.ReadAsync<MHWildsEncryptedFloat>(part.HealthPointer);
            MHWildsEncryptedFloat encryptedMaxHealth = await Memory.ReadAsync<MHWildsEncryptedFloat>(part.MaxHealthPointer);

            var data = new UpdatePartData
            {
                Health = await _cryptoService.DecryptFloat(encryptedHealth),
                MaxHealth = await _cryptoService.DecryptFloat(encryptedMaxHealth)
            };

            bool isNewPart = index >= _parts.Count;

            if (isNewPart)
            {
                MonsterPartDefinition definition = index < _definition.Parts.Length
                    ? _definition.Parts[index]
                    : MonsterRepository.UnknownPartDefinition;

                _parts.Add(new MHWildsMonsterPart(definition));
            }

            MHWildsMonsterPart partEntity = _parts[index];

            partEntity.Update(data);

            if (isNewPart)
                this.Dispatch(
                    toDispatch: _onNewPartFound,
                    data: partEntity
                );
            index++;
        }
    }

    [ScannableMethod]
    internal Task FinishInitializationAsync()
    {
        if (_isInitialized)
            return Task.CompletedTask;

        _logger.Debug($"Initialized {Name} at address {_address:X} with id: {Id}");
        _isInitialized = true;

        this.Dispatch(
            toDispatch: _onWeaknessesChange,
            data: Weaknesses
        );

        if (Health > 0)
            this.Dispatch(
                toDispatch: _onSpawn,
                data: EventArgs.Empty
            );

        return Task.CompletedTask;
    }
}