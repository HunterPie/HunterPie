using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise;

public sealed class MHRContext(
    IGameProcess process,
    IScanService scanService,
    ILocalizationRepository localizationRepository
) : Context(
    game: new MHRGame(
        process: process,
        scanService: scanService,
        localizationRepository: localizationRepository
    ),
    process: process
);