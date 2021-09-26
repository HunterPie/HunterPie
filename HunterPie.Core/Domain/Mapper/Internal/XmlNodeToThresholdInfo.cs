using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal
{
    class XmlNodeToThresholdInfo : IMapper<XmlNode, ThresholdInfo>
    {
        public ThresholdInfo Map(XmlNode data)
        {
            bool hasConditions = bool.Parse(data.Attributes["HasConditions"]?.Value ?? "False");
            ThresholdInfo info = new ThresholdInfo
            {
                Threshold = int.Parse(data.Attributes["Threshold"].Value),
                HasConditions = hasConditions,
                MinFlinch = hasConditions ? int.Parse(data.Attributes["MinFlinch"]?.Value ?? "0") : 0,
                MinHealth = hasConditions ? int.Parse(data.Attributes["MinHealth"]?.Value ?? "100") : 100
            };

            return info;
        }
    }
}
