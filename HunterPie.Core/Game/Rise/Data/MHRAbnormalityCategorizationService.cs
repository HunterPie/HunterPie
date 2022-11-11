using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Rise.Data;
public class MHRAbnormalityCategorizationService : IAbnormalityCategorizationService
{
    public AbnormalityCategory Categorize(IAbnormality abnormality)
    {
        return (abnormality.Id) switch
        {
            "ABN_NATURAL_HEALING" or "ABN_GOURMET_FISH" => AbnormalityCategory.NaturalHealing,
            "ABN_POISON" or "ABN_VENOM" => AbnormalityCategory.Poison,
            "ABN_FIRE" => AbnormalityCategory.Fire,
            "ABN_WATER" => AbnormalityCategory.Water,
            "ABN_ICE" => AbnormalityCategory.Ice,
            "ABN_BLEED" => AbnormalityCategory.Bleed,
            _ => AbnormalityCategory.None
        };
    }
}
