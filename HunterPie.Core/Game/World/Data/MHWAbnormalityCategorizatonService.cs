using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.World.Data;
public class MHWAbnormalityCategorizatonService : IAbnormalityCategorizationService
{
    public AbnormalityCategory Categorize(IAbnormality abnormality)
    {
        return (abnormality.Icon) switch
        {
            "ICON_POISON" or "ICON_VENOM" => AbnormalityCategory.Poison,
            "ELEMENT_FIRE" => AbnormalityCategory.Fire,
            "ELEMENT_WATER" => AbnormalityCategory.Water,
            "ELEMENT_ICE" => AbnormalityCategory.Ice,
            "ICON_BLEED" => AbnormalityCategory.Bleed,
            "ICON_EFFLUVIA" => AbnormalityCategory.Effluvia,
            "ICON_NATURALHEALING" => AbnormalityCategory.NaturalHealing,
            _ => AbnormalityCategory.None
        };
    }
}
