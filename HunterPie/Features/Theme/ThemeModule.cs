using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Theme.Datasources;
using HunterPie.Features.Theme.Loader;

namespace HunterPie.Features.Theme;

internal class ThemeModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<LocalThemeService>()
            .WithSingle<ThemeLoaderService>();
    }
}