using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Extensions.Controller;
using HunterPie.Features.Extensions.Loader;
using HunterPie.Features.Extensions.Navigation;
using HunterPie.Features.Extensions.Repository;
using HunterPie.Features.Extensions.ViewModels;

namespace HunterPie.Features.Extensions;

internal class ThemeModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<LocalThemeRepository>()
            .WithSingle<ThemeLoaderService>()
            .WithFactory<ThemeHomeController>()
            .WithSingle<ThemeNavigationHandler>()
            .WithFactory<InstalledPluginViewModel>();
    }
}