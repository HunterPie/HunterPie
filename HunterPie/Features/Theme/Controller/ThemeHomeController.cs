using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Features.Theme.Datasources;
using HunterPie.Features.Theme.Entity;
using HunterPie.Features.Theme.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.Controller;

internal class ThemeHomeController
{
    private readonly LocalThemeService _localThemeService;
    private readonly V5Config _config;

    public ThemeHomeController(
        LocalThemeService localThemeService,
        V5Config config)
    {
        _localThemeService = localThemeService;
        _config = config;
    }

    public async Task<ThemeHomeViewModel> GetViewModelAsync()
    {
        var viewModel = new ThemeHomeViewModel();

        var tab = new ExploreThemeHomeTabViewModel { Title = "Discover" };

        for (int i = 0; i < 10; i++)
            tab.Themes.Add(
                item: new ThemeCardViewModel()
            );

        viewModel.Tabs.Add(
            item: await GetInstalledTabViewModelAsync()
        );


        return viewModel;
    }

    private async Task<InstalledThemeHomeTabViewModel> GetInstalledTabViewModelAsync()
    {
        IReadOnlyCollection<LocalThemeManifest> themes = await _localThemeService.ListAllAsync();

        var installedTab = new InstalledThemeHomeTabViewModel { Title = "Installed" };

        foreach (LocalThemeManifest theme in themes)
            installedTab.Themes.Add(new InstalledThemeViewModel
            {
                Name = theme.Manifest.Name,
                Author = theme.Manifest.Author,
                Path = theme.Path,
                IsEnabled = _config.Client.Themes.Contains(theme.Path),
                IsDraggingOver = false
            });

        return installedTab;
    }
}