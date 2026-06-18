using HunterPie.Core.Client.Localization;
using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Entity;
using HunterPie.UI.Client.Sidebar.Handler;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Github.Navigation;

internal class GitHubNavigationHandler(
    ILocalizationRepository localizationRepository
) : NavigationHandler(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='GITHUB_STRING']"),
    icon: Resources.Icon("ICON_GITHUB")
)
{
    public override Task InitializeAsync()
    {
        State = SideBarButtonState.Enabled;
        return Task.CompletedTask;
    }

    public override Task ExecuteAsync()
    {
        BrowserService.OpenUrl(CommonLinks.GITHUB);

        return Task.CompletedTask;
    }
}