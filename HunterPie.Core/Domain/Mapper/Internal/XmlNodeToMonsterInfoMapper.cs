using System;
using System.Xml;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data;
using System.Globalization;
using System.Linq;

namespace HunterPie.Core.Domain.Mapper.Internal
{
    class XmlNodeToMonsterInfoMapper : IMapper<XmlNode, MonsterInfo>
    {
        public MonsterInfo Map(XmlNode data)
        {
            if (data?.Attributes == null)
                throw new ArgumentNullException(nameof(data), "XmlNode and its attributes cannot be null!");

            CrownInfo monsterCrowns = new()
            {
                Mini = float.Parse(data.SelectSingleNode("Crown/@Mini")?.Value ?? "0.9", CultureInfo.InvariantCulture),
                Silver = float.Parse(data.SelectSingleNode("Crown/@Silver")?.Value ?? "1.15", CultureInfo.InvariantCulture),
                Gold = float.Parse(data.SelectSingleNode("Crown/@Gold")?.Value ?? "1.23", CultureInfo.InvariantCulture)
            };

            MonsterInfo info = new()
            {
                Em = data.Attributes["Id"].Value,
                Id = int.Parse(data.Attributes["GameId"].Value),
                Crowns = monsterCrowns,
                Capture = float.Parse(data.Attributes["Capture"].Value ?? "0"),
                MaxParts = int.Parse(data.SelectSingleNode("Parts/@Max")?.Value ?? "0"),
            };

            info.Weaknesses = data.SelectNodes("Weaknesses/Weakness")!
                .Cast<XmlNode>()
                .Select(node => MapFactory.Map<XmlNode, WeaknessInfo>(node))
                .ToArray();


            return info;
        }
    }
}
