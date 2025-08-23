using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Extensions;
using HunterPie.Features.Theme.Entity;
using HunterPie.Features.Theme.Repository;
using HunterPie.Features.Theme.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.Controller;

internal class ThemeHomeController
{
    private readonly LocalThemeRepository _localThemeRepository;
    private readonly V5Config _config;

    public ThemeHomeController(
        LocalThemeRepository localThemeRepository,
        V5Config config)
    {
        _localThemeRepository = localThemeRepository;
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
        IReadOnlyCollection<LocalThemeManifest> themes = await _localThemeRepository.ListAllAsync();

        var installedTab = new InstalledThemeHomeTabViewModel { Title = "Installed" };

        foreach (LocalThemeManifest theme in themes)
            installedTab.Themes.Add(new InstalledThemeViewModel
            {
                Id = theme.Manifest.Id,
                Name = theme.Manifest.Name,
                Description = theme.Manifest.Description,
                Author = theme.Manifest.Author,
                Version = theme.Manifest.Version,
                Path = theme.Path,
                IsEnabled = _config.Client.Themes.Contains(theme.Manifest.Id),
                IsDraggingOver = false,
                Tags = theme.Manifest.Tags.ToObservableCollection()
            });

        return installedTab;
    }
}