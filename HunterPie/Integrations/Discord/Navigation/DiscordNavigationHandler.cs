using HunterPie.Core.Client.Localization;
using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Handler;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Discord.Navigation;

internal class DiscordNavigationHandler(
    ILocalizationRepository localizationRepository
) : NavigationHandler.Action(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='DISCORD_STRING']"),
    icon: Resources.Icon("ICON_DISCORD")
)
{
    public override Task ExecuteAsync()
    {
        BrowserService.OpenUrl(CommonLinks.DISCORD);

        return Task.CompletedTask;
    }
}