using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Features.Plugins.Services;
using HunterPie.Features.Theme.Repository;
using HunterPie.Features.Theme.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.Controller;

internal class ThemeHomeController(
    LocalThemeRepository localThemeRepository,
    PluginProvider pluginProvider,
    V5Config config)
{
    public async Task<ThemeHomeViewModel> GetViewModelAsync()
    {
        var viewModel = new ThemeHomeViewModel();

        viewModel.Tabs.Add(
            item: await GetInstalledTabViewModelAsync()
        );
        viewModel.Tabs.Add(
            item: await GetInstalledPluginsTabViewModelAsync()
        );

        return viewModel;
    }

    private async Task<InstalledThemeHomeTabViewModel> GetInstalledTabViewModelAsync()
    {
        var installedTab = new InstalledThemeHomeTabViewModel(
            configuredThemes: config.Client.Themes,
            localThemeRepository: localThemeRepository
        )
        {
            Icon = "Icons.Palette",
            Title = "Themes"
        };

        await installedTab.RefreshAsync();

        return installedTab;
    }

    private async Task<InstalledPluginsHomeTabViewModel> GetInstalledPluginsTabViewModelAsync()
    {
        var installedPluginsTab = new InstalledPluginsHomeTabViewModel(pluginProvider)
        {
            Icon = "Icons.Plugin",
            Title = "Plugins"
        };

        await installedPluginsTab.RefreshAsync();

        return installedPluginsTab;
    }
}