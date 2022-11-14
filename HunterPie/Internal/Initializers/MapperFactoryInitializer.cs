using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Domain.Mapper.Internal;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class MapperFactoryInitializer : IInitializer
{
    public Task Init()
    {
        MapFactory.Add(new GameTypeToGameProcessMapper());

        return Task.CompletedTask;
    }
}
