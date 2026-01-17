using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Services.Monster;

namespace HunterPie.UI.Architecture.Adapter;

public class MonsterNameAdapter(ILocalizationRepository localizationRepository)
{
    private readonly ILocalizationRepository _localizationRepository = localizationRepository;

    public string From(
        GameType game,
        int monsterId,
        VariantType variant)
    {
        var nameService = new MonsterVariantNameService(
            localizationRepository: _localizationRepository,
            monsterNamesRepository: _localizationRepository.WithScope($"//Strings/Monsters/{game}/Monster")
        );

        return nameService.GetName(
            id: monsterId,
            variant: variant
        );
    }
}