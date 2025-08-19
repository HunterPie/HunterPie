using HunterPie.Features.Theme.ViewModels;

namespace HunterPie.Features.Theme.Controller;

internal class ThemeHomeController
{
    public ThemeHomeViewModel GetViewModel()
    {
        var viewModel = new ThemeHomeViewModel();

        string[] tabs = { "Discover", "Installed" };

        var tab = new ExploreThemeHomeTabViewModel { Title = "Discover" };

        for (int i = 0; i < 10; i++)
            tab.Themes.Add(
                item: new ThemeCardViewModel()
            );

        viewModel.Tabs.Add(tab);

        var installedTab = new InstalledThemeHomeTabViewModel { Title = "Installed" };

        for (int i = 0; i < 7; i++)
            installedTab.Themes.Add(
                item: new InstalledThemeViewModel
                {
                    IsEnabled = true,
                    Name = $"Theme {i}"
                }
            );

        viewModel.Tabs.Add(installedTab);

        return viewModel;
    }
}