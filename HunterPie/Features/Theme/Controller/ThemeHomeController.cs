using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features.Repository;
using HunterPie.Features.Plugins.Repository;
using HunterPie.Features.Theme.Repository;
using HunterPie.Features.Theme.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.Controller;

internal class ThemeHomeController(
    LocalThemeRepository localThemeRepository,
    IPluginRepository pluginRepository,
    IFeatureFlagRepository flagRepository,
    V5Config config
)
{
    private bool IsPluginViewEnabled => flagRepository.IsEnabled(FeatureFlags.FEATURE_IS_PLUGINS_VIEW_ENABLED);


    public async Task<ThemeHomeViewModel> GetViewModelAsync()
    {
        var viewModel = new ThemeHomeViewModel();

        viewModel.Tabs.Add(
            item: await GetInstalledTabViewModelAsync()
        );

        if (IsPluginViewEnabled)
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
        var installedPluginsTab = new InstalledPluginsHomeTabViewModel(pluginRepository)
        {
            Icon = "Icons.Plugin",
            Title = "Plugins"
        };

        await installedPluginsTab.RefreshAsync();

        return installedPluginsTab;
    }
}