using HunterPie.Core.Game.Data.Interfaces;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Services;
public class MHRAbnormalityFlagTypeParser : IAbnormalityFlagTypeParser
{
    public Enum? Parse(AbnormalityFlagType type, string value)
    {
        Type? enumType = type switch
        {
            AbnormalityFlagType.None => null,
            AbnormalityFlagType.RiseCommon => typeof(CommonConditions),
            AbnormalityFlagType.RiseDebuff => typeof(DebuffConditions),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        if (enumType is null)
            return null;

        return (Enum)Enum.Parse(enumType, value);
    }
}
