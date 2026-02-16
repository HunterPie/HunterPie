using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld;

public sealed class MHWContext(
    IGameProcess process,
    IScanService scanService,
    ILocalizationRepository localizationRepository
) : Context(
    game: new MHWGame(
        process: process,
        scanService: scanService,
        localizationRepository: localizationRepository
    ),
    process: process
);