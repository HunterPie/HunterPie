using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game;

public sealed class MHWildsMonster : CommonMonster
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly nint _address;

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

            _logger.Debug($"Monster health: {value:0}/{MaxHealth:0}");
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

    public override bool IsEnraged { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

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

    public override IReadOnlyCollection<IMonsterAilment> Ailments => Array.Empty<IMonsterAilment>();

    public override IMonsterAilment Enrage => throw new NotImplementedException();

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
        MHWildsMonsterBasicData basicData) : base(process, scanService)
    {
        _address = address;
        Id = basicData.Id;

        _logger.Debug($"Found monster with ID {Id} @ {address:X8}");
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

        MaxHealth = await Decrypt(encryptedMaxHealth);
        Health = await Decrypt(encryptedHealth);
    }

    private async Task<float> Decrypt(MHWildsEncryptedFloat encrypted)
    {
        ulong[] encryptionKey = await Memory.ReadAsync<ulong>(
            address: AddressMap.GetAbsolute("Encryption::Key"),
            count: 2
        );
        ulong[] roundKey = await Memory.ReadAsync<ulong>(
            address: AddressMap.GetAbsolute("Encryption::Round"),
            count: 2
        );

        Vector128<byte> encryptionKeyVec = Vector128.Create(encryptionKey).AsByte();
        Vector128<byte> roundKeyVec = Vector128.Create(roundKey).AsByte();

        Vector128<byte> xorValues = encryptionKeyVec ^ encrypted.ToVector128();

        Vector128<byte> decryptedValues = Aes.DecryptLast(xorValues, roundKeyVec);

        return decryptedValues.AsSingle()[0];
    }
}