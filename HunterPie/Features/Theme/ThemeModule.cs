using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Theme.Controller;
using HunterPie.Features.Theme.Loader;
using HunterPie.Features.Theme.Repository;

namespace HunterPie.Features.Theme;

internal class ThemeModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<LocalThemeRepository>()
            .WithSingle<ThemeLoaderService>()
            .WithFactory<ThemeHomeController>();
    }
}