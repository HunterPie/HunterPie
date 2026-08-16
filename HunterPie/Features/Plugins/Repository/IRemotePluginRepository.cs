using HunterPie.Features.Plugins.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Features.Plugins.Repository;

internal interface IRemotePluginRepository
{
    public Task<List<AvailablePlugin>> List(string cursor);
}
