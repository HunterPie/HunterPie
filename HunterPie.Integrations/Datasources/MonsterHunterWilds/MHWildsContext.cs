using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds;

public class MHWildsContext(
    IGameProcess process,
    IScanService scanService,
    ILocalizationRepository localizationRepository,
    MHWildsMonsterTargetKeyManager monsterTargetKeyManager
) : Context(
    game: new MHWildsGame(
        process: process,
        scanService: scanService,
        localizationRepository: localizationRepository,
        monsterTargetKeyManager: monsterTargetKeyManager
    ),
    process: process
);