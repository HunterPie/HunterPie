using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Data;
using System;
using System.Xml;

namespace HunterPie.Core.Domain.Mapper.Internal
{
    class XmlNodeToWeaknessInfoMapper : IMapper<XmlNode, WeaknessInfo>
    {
        public WeaknessInfo Map(XmlNode data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data), "XmlNode cannot be null");

            WeaknessInfo info = new()
            {
                Id = data.Attributes["Id"]?.Value,
                Stars = int.Parse(data.Attributes["Stars"]?.Value ?? "0")
            };

            return info;
        }
    }
}
