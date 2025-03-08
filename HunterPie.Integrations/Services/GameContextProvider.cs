using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Services.Exceptions;

namespace HunterPie.Integrations.Services;

internal class GameContextProvider : IGameContextService
{
    private readonly IScanService _scanService;
    private readonly ILocalizationRepository _localizationRepository;

    public GameContextProvider(
        IScanService scanService,
        ILocalizationRepository localizationRepository)
    {
        _scanService = scanService;
        _localizationRepository = localizationRepository;
    }

    public Context Get(IGameProcess game)
    {
        return game.Type switch
        {
            GameProcessType.MonsterHunterRise => new MHRContext(game, _scanService),

            GameProcessType.MonsterHunterWorld => new MHWContext(game, _scanService),

            GameProcessType.MonsterHunterWilds => new MHWildsContext(game, _scanService, _localizationRepository),

            GameProcessType.None or
            GameProcessType.All or
            _ => throw new UnsupportedGameException(game.Name)
        };
    }
}