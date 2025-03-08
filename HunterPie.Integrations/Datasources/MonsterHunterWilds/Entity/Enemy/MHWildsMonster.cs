using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration.Enums;
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
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public sealed class MHWildsMonster : CommonMonster
{
    private bool _isInitialized;
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly MHWildsCryptoService _cryptoService;
    private readonly nint _address;
    private readonly MHWildsMonsterAilment _enrage = new(MonsterAilmentRepository.Enrage);

    private readonly List<MHWildsMonsterAilment> _ailments = new(45);

    public override string Name => "Unknown";

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

            if (Health <= 0)
                this.Dispatch(_onDeath);
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

    public override IMonsterPart[] Parts => Array.Empty<IMonsterPart>();

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

    public override Element[] Weaknesses => Array.Empty<Element>();

    public override string[] Types => Array.Empty<string>();

    public MHWildsMonster(
        IGameProcess process,
        IScanService scanService,
        nint address,
        MHWildsMonsterBasicData basicData,
        MHWildsCryptoService cryptoService) : base(process, scanService)
    {
        _address = address;
        Id = basicData.Id;
        _cryptoService = cryptoService;
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
        MHWildsBuildUp stamina = await Memory.ReadAsync<MHWildsBuildUp>(ailment.BuildUpPointer);

        MaxStamina = stamina.Max;
        Stamina = stamina.Current;
    }

    [ScannableMethod]
    internal async Task GetEnrageAsync()
    {
        MHWildsAilment ailment = await Memory.DerefPtrAsync<MHWildsAilment>(
            address: _address,
            offsets: AddressMap.GetOffsets("Monster::Enrage")
        );

        MHWildsBuildUp buildUp = await Memory.ReadAsync<MHWildsBuildUp>(ailment.BuildUpPointer);

        _enrage.Update(ailment.Timer);
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

            ailmentEntity.Update(ailment.Timer);
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
    internal Task FinishInitializationAsync()
    {
        if (_isInitialized)
            return Task.CompletedTask;

        _logger.Debug($"Initialized {Name} at address {_address:X} with id: {Id}");
        _isInitialized = true;

        this.Dispatch(
            toDispatch: _onSpawn,
            data: EventArgs.Empty
        );

        return Task.CompletedTask;
    }
}