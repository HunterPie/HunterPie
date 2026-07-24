using HunterPie.Core.Client.Localization;
using HunterPie.Features.Extensions.Controller;
using HunterPie.Features.Extensions.ViewModels;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Entity;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Features.Extensions.Navigation;

internal class ThemeNavigationHandler(
    IBodyNavigator navigator,
    ILocalizationRepository localizationRepository,
    ThemeHomeController themeHomeController
) : NavigationHandler.View<ThemeHomeViewModel>(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='EXTENSIONS_STRING']"),
    icon: Resources.Icon("Icons.Plugin")
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