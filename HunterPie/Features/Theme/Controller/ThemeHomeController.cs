using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Features.Theme.Repository;
using HunterPie.Features.Theme.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.Controller;

internal class ThemeHomeController(
    LocalThemeRepository localThemeRepository,
    V5Config config)
{
    private readonly LocalThemeRepository _localThemeRepository = localThemeRepository;
    private readonly V5Config _config = config;

    public async Task<ThemeHomeViewModel> GetViewModelAsync()
    {
        var viewModel = new ThemeHomeViewModel();

        viewModel.Tabs.Add(
            item: await GetInstalledTabViewModelAsync()
        );

        return viewModel;
    }

    private async Task<InstalledThemeHomeTabViewModel> GetInstalledTabViewModelAsync()
    {
        var installedTab = new InstalledThemeHomeTabViewModel(
            configuredThemes: _config.Client.Themes,
            localThemeRepository: _localThemeRepository
        )
        {
            Icon = "Icons.Palette",
            Title = "Installed"
        };

        await installedTab.RefreshAsync();

        return installedTab;
    }
}