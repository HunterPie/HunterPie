using HunterPie.Core.Client.Localization;
using HunterPie.Features.Theme.Controller;
using HunterPie.Features.Theme.ViewModels;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Entity;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.Navigation;

internal class ThemeNavigationHandler(
    IBodyNavigator navigator,
    ILocalizationRepository localizationRepository,
    ThemeHomeController themeHomeController
) : NavigationHandler<ThemeHomeViewModel>(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='THEMES_STRING']"),
    icon: Resources.Icon("Icons.Palette")
)
{
    public override Task InitializeAsync()
    {
        State = SideBarButtonState.Enabled;
        return Task.CompletedTask;
    }

    public override async Task ExecuteAsync()
    {
        navigator.Navigate(
            viewModel: await themeHomeController.GetViewModelAsync()
        );
    }
}