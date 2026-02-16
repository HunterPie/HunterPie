using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Services.Exceptions;

namespace HunterPie.Integrations.Services;

internal class GameContextProvider(
    IScanService scanService,
    ILocalizationRepository localizationRepository) : IGameContextService
{

    public Context Get(IGameProcess game)
    {
        return game.Type switch
        {
            GameProcessType.MonsterHunterRise => new MHRContext(
                process: game,
                scanService: scanService,
                localizationRepository: localizationRepository
            ),

            GameProcessType.MonsterHunterWorld => new MHWContext(
                process: game,
                scanService: scanService,
                localizationRepository: localizationRepository
            ),

            GameProcessType.MonsterHunterWilds => new MHWildsContext(
                process: game,
                scanService: scanService,
                localizationRepository: localizationRepository,
                monsterTargetKeyManager: DependencyContainer.Get<MHWildsMonsterTargetKeyManager>()
            ),

            GameProcessType.None or
            GameProcessType.All or
            _ => throw new UnsupportedGameException(game.Name)
        };
    }
}