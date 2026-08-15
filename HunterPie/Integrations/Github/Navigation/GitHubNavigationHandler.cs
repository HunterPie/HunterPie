using HunterPie.Core.Client.Localization;
using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Handler;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Github.Navigation;

internal class GitHubNavigationHandler(
    ILocalizationRepository localizationRepository
) : NavigationHandler.Action(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='GITHUB_STRING']"),
    icon: Resources.Icon("ICON_GITHUB")
)
{
    public override Task ExecuteAsync()
    {
        BrowserService.OpenUrl(CommonLinks.GITHUB);

        return Task.CompletedTask;
    }
}