using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;
using HunterPie.Core.Game.Entity.Enemy;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;
public sealed class MHWildsMonsterNameGenerator
{
    private const string VARIANT_PATH = "//Strings/Monsters/Variants/Variant[@Id='{0}']";
    private const string MONSTER_PATH = "//Strings/Monsters/Wilds/Monster[@Id='{0}']";

    private readonly ILocalizationRepository _localizationRepository;

    public MHWildsMonsterNameGenerator(ILocalizationRepository localizationRepository)
    {
        _localizationRepository = localizationRepository;
    }

    public string GetName(
        int id,
        VariantType variant)
    {
        IEnumerable<LocalizationData> variantTypes =
            Enum.GetValues<VariantType>()
            .Where(variantType =>
                variant.HasFlag(variantType) &&
                variantType != VariantType.Normal)
            .Select(GetVariantData);

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
        string namePath = string.Format(MONSTER_PATH, id);

        string name = _localizationRepository.ExistsBy(namePath)
            ? _localizationRepository.FindStringBy(namePath)
            : $"Unknown [id: {id}]";

        return name;
    }

    private LocalizationData GetVariantData(VariantType variant)
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