using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal
{
    class XmlNodeToPartInfoMapper : IMapper<XmlNode, PartInfo[]>
    {
        static private uint[] ParseTenderizedIdsToArray(string tenderizedIds)
        {
            if (tenderizedIds == "") 
                return Array.Empty<uint>();
            
            string[] ids = tenderizedIds.Split(',');
            uint[] parsed = new uint[ids.Length];
            uint i = 0;
            
            foreach (string id in ids)
            {
                parsed[i] = Convert.ToUInt32(id);
                i++;
            }

            return parsed;
        }

        public PartInfo[] Map(XmlNode data)
        {
            List<PartInfo> parts = new List<PartInfo>();
            if (data == null) return null;

            XmlNodeList mParts = data.SelectNodes("Parts/Part");
            uint RemovablePartIndex = 0;
            foreach (XmlNode partData in mParts)
            {
                PartInfo pInfo = new PartInfo
                {
                    Id = partData.Attributes["Name"]?.Value ?? "MONSTER_PART_UNKNOWN",
                    IsRemovable = bool.Parse(partData.Attributes["IsRemovable"]?.Value ?? "false"),
                    GroupId = partData.Attributes["Group"]?.Value ?? "MISC",
                    Skip = bool.Parse(partData.Attributes["Skip"]?.Value ?? "false"),
                    Index = uint.Parse(partData.Attributes["Index"]?.Value ?? RemovablePartIndex.ToString()),
                    TenderizeIds = ParseTenderizedIdsToArray(partData.Attributes["TenderizeIds"]?.Value ?? "")
                };

                if (pInfo.IsRemovable) 
                    RemovablePartIndex++;

                pInfo.BreakThresholds = partData.SelectNodes("Break")
                    .Cast<XmlNode>()
                    .Select(node => MapFactory.Map<XmlNode, ThresholdInfo>(node))
                    .ToArray();

                parts.Add(pInfo);
            }

            return parts.ToArray();
        }
    }
}
