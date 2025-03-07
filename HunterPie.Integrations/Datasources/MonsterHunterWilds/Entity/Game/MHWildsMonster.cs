using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game;

public sealed class MHWildsMonster : CommonMonster
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly nint _address;

    public override string Name => throw new NotImplementedException();

    public override int Id { get; protected set; }

    public override float Health { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    public override float MaxHealth { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    public override float Stamina { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    public override float MaxStamina { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    public override float CaptureThreshold { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    public override bool IsEnraged { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    public override Target Target { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    public override Target ManualTarget { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

    public override IMonsterPart[] Parts => throw new NotImplementedException();

    public override IReadOnlyCollection<IMonsterAilment> Ailments => throw new NotImplementedException();

    public override IMonsterAilment Enrage => throw new NotImplementedException();

    public override Crown Crown { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

    public override Element[] Weaknesses => throw new NotImplementedException();

    public override string[] Types => throw new NotImplementedException();

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
}