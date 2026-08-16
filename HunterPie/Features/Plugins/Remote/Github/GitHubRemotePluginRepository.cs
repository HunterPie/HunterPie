using HunterPie.Core.Networking.Http;
using HunterPie.Features.Plugins.Entity;
using HunterPie.Features.Plugins.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Features.Plugins.Remote.Github;

internal class GitHubRemotePluginRepository(HttpClientBuilder client) : IRemotePluginRepository
{

    public Task<List<AvailablePlugin>> List(string cursor)
    {
        client.Post

        return [];
    }
}
