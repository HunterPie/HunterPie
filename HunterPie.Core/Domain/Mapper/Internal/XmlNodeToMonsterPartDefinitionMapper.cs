using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using System;
using System.Collections.Generic;
using System.IO;
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
            TenderizeIds = tenderizeIds,
            Group = MonsterPartRepository.FindBy(localizationId ?? string.Empty) ?? throw new InvalidDataException($"Part group for '{localizationId}' not found"),
        };

        int.TryParse(id, out output.Id);
        bool.TryParse(isSeverableFlag, out output.IsSeverable);

        var breakThresholds = new List<int>();
        foreach (XmlNode node in data.ChildNodes)
        {
            if (node.Name != "Break")
                continue;

            string? rawThreshold = node.Attributes?["Threshold"]?.Value;

            if (int.TryParse(rawThreshold, out int threshold))
                breakThresholds.Add(threshold);
        }

        output.BreakThresholds = breakThresholds;

        return output;
    }
}