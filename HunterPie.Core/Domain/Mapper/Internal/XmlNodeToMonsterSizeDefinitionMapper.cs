using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data.Definitions;
using System.Globalization;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal;

public class XmlNodeToMonsterSizeDefinitionMapper : IMapper<XmlNode, MonsterSizeDefinition>
{
    private const float MINI_CROWN = 0.9f;
    private const float SILVER_CROWN = 1.15f;
    private const float GOLD_CROWN = 1.23f;

    public MonsterSizeDefinition Map(XmlNode data)
    {
        var schema = new MonsterSizeDefinition
        {
            Mini = MINI_CROWN,
            Silver = SILVER_CROWN,
            Gold = GOLD_CROWN
        };

        XmlNode? crownsNode = data.SelectSingleNode("Crowns");

        float.TryParse(
            data.Attributes?["Size"]?.Value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out schema.Size
        );

        float.TryParse(
            crownsNode?.Attributes?["Mini"]?.Value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out schema.Mini
        );

        float.TryParse(
            crownsNode?.Attributes?["Silver"]?.Value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out schema.Silver
        );

        float.TryParse(
            crownsNode?.Attributes?["Gold"]?.Value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out schema.Gold
        );

        return schema;
    }
}