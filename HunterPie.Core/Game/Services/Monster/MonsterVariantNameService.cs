using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;
using HunterPie.Core.Game.Entity.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.Services.Monster;

public sealed class MonsterVariantNameService(
    ILocalizationRepository localizationRepository,
    IScopedLocalizationRepository monsterNamesRepository)
{
    private const string VARIANT_PATH = "//Strings/Monsters/Variants/Variant[@Id='{0}']";

    private readonly ILocalizationRepository _localizationRepository = localizationRepository;
    private readonly IScopedLocalizationRepository _monsterNamesRepository = monsterNamesRepository;

    public string GetName(
        int id,
        VariantType variant)
    {
        IEnumerable<LocalizationData> variantTypes =
            Enum.GetValues<VariantType>()
                .Where(variantType =>
                    variant.HasFlag(variantType) &&
                    variantType != VariantType.Normal)
                .Select(GetVariant);

        string prefix =
            string
                .Join(
                    " ",
                    (variantTypes
                        .Where(x => x.Affixation == Affixation.Prefix)
                        .OrderBy(x => x.Order)
                        .Select(x => x.String)
                    )
                );

        string suffix =
            string
                .Join(
                    " ",
                    (variantTypes
                        .Where(x => x.Affixation == Affixation.Suffix)
                        .OrderBy(x => x.Order)
                        .Select(x => x.String)
                    )
                );

        string monsterName = GetMonsterName(id);

        return $"{prefix} {monsterName} {suffix}";
    }

    private string GetMonsterName(int id)
    {
        return _monsterNamesRepository.TryFindBy(id.ToString())?.String ?? $"Unknown [id: {id}]";
    }

    private LocalizationData GetVariant(VariantType variant)
    {
        string path = GetPathToVariant(variant);

        return _localizationRepository.FindBy(path);
    }

    private static string GetPathToVariant(VariantType variant)
    {
        return variant switch
        {
            VariantType.Frenzy => string.Format(VARIANT_PATH, "FRENZIED"),
            VariantType.Tempered => string.Format(VARIANT_PATH, "TEMPERED"),
            VariantType.ArchTempered => string.Format(VARIANT_PATH, "ARCH_TEMPERED"),
            _ => string.Format(VARIANT_PATH, "NORMAL"),
        };
    }
}