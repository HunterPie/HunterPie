using HunterPie.Features.Plugins.Entity;
using HunterPie.Features.Plugins.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Features.Plugins.Remote.Github;

internal class GitHubRemotePluginRepository() : IRemotePluginRepository
{
    public Task<List<AvailablePlugin>> List(string cursor)
    {
        throw new NotImplementedException();
    }
}
