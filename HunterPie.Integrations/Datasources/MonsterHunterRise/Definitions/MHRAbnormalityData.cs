using HunterPie.Core.Game.Data.Schemas;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
public struct MHRAbnormalityData
{
    public float Timer;
}

public static class MHRAbnormalityAdapter
{
    public static MHRAbnormalityData Convert(AbnormalitySchema schema, MHRAbnormalityStructure abnormality)
    {
        if (schema.IsInteger)
            return new MHRAbnormalityData { Timer = (float)abnormality.Timer };

        return schema.Category switch
        {
            "Consumables" => new MHRAbnormalityData { Timer = abnormality.Consumable.Timer },
            "Debuffs" => new MHRAbnormalityData { Timer = abnormality.Debuff.Timer },
            _ => new MHRAbnormalityData()
        };
    }
}