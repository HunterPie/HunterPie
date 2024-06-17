using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data.Definitions;
using System;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal;

public class XmlNodeToMonsterPartDefinitionMapper : IMapper<XmlNode, MonsterPartDefinition>
{
    public MonsterPartDefinition Map(XmlNode data)
    {
        string? id = data.Attributes?["Id"]?.Value;
        string? localizationId = data.Attributes?["String"]?.Value;
        string? rawTenderizeIds = data.Attributes?["TenderizeIds"]?.Value;
        string? isSeverableFlag = data.Attributes?["IsSeverable"]?.Value;

        uint[] tenderizeIds = rawTenderizeIds switch
        {
            null => Array.Empty<uint>(),
            _ => rawTenderizeIds.Split(',')
                .Select(uint.Parse)
                .ToArray()
        };

        var output = new MonsterPartDefinition
        {
            String = localizationId ?? string.Empty,
            TenderizeIds = tenderizeIds
        };

        int.TryParse(id, out output.Id);
        bool.TryParse(isSeverableFlag, out output.IsSeverable);

        return output;
    }
}