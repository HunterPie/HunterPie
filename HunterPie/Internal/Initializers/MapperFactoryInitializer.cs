using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Domain.Mapper.Internal;
using HunterPie.Domain.Interfaces;

namespace HunterPie.Internal.Initializers
{
    internal class MapperFactoryInitializer : IInitializer
    {
        public void Init()
        {
            MapFactory.Add(new GameTypeToGameProcessMapper());
        }
    }
}
