using HunterPie.Core.Client.Localization;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Entity;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Logging.ViewModels;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.UI.Logging.Navigation;

internal class ConsoleNavigationHandler(
    IBodyNavigator bodyNavigator,
    ConsoleViewModel consoleViewModel,
    ILocalizationRepository localizationRepository
) : NavigationHandler.View<ConsoleViewModel>(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='CONSOLE_STRING']"),
    icon: Resources.Icon("ICON_CONSOLE")
)
{
    public override Task InitializeAsync()
    {
        State = SideBarButtonState.Enabled;
        return Task.CompletedTask;
    }

    public override Task ExecuteAsync()
    {
        bodyNavigator.Navigate(consoleViewModel);

        return Task.CompletedTask;
    }
}