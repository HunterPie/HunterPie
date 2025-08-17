using HunterPie.Features.Theme.ViewModels;

namespace HunterPie.Features.Theme.Controller;

internal class ThemeHomeController
{
    public ThemeHomeViewModel GetViewModel()
    {
        var viewModel = new ThemeHomeViewModel();

        string[] tabs = { "Discover", "Installed" };

        foreach (string tabTitle in tabs)
        {
            var tab = new ThemeHomeTabViewModel { Title = tabTitle };

            for (int i = 0; i < 10; i++)
                tab.Themes.Add(
                    item: new ThemeCardViewModel()
                );

            viewModel.Tabs.Add(tab);
        }

        return viewModel;
    }
}