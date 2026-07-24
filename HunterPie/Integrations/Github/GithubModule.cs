using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Github.Navigation;

namespace HunterPie.Integrations.Github;

internal class GithubModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithSingle<GitHubNavigationHandler>();
    }
}