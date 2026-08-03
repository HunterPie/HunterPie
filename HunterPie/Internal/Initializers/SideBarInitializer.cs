using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Extensions.Navigation;
using HunterPie.Features.Patches.Navigation;
using HunterPie.Features.Settings.Navigation;
using HunterPie.Features.Statistics.Navigation;
using HunterPie.Integrations.Discord.Navigation;
using HunterPie.Integrations.Github.Navigation;
using HunterPie.Integrations.Patreon.Navigation;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Client.Sidebar.Service;
using HunterPie.UI.Home.Navigation;
using HunterPie.UI.Logging.Navigation;
using HunterPie.UI.SideBar.Services;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class SideBarInitializer(
    IDependencyRegistry dependencies,
    ISideBarRegistry sidebarRegistry,
    IFixedSideBarRegistry fixedSidebarRegistry,
    ILocalizationRepository localizationRepository
) : IInitializer
{
    public async Task Init()
    {
        NavigationHandler[] handlers = [
            dependencies.Get<HomeNavigationHandler>(),
            dependencies.Get<ConsoleNavigationHandler>(),
            dependencies.Get<ThemeNavigationHandler>(),
            dependencies.Get<PatchNotesNavigationHandler>(),
            dependencies.Get<QuestStatisticsNavigationHandler>(),

        ];

        NavigationHandler[] fixedHandlers = [
            new NavigationHandler.Group(
                label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='LINKS_STRING']"),
                icon: Resources.Icon("Icons.Link"),
                [
                    dependencies.Get<DiscordNavigationHandler>(),
                    dependencies.Get<PatreonNavigationHandler>(),
                    dependencies.Get<GitHubNavigationHandler>()
                ]
            ),
            dependencies.Get<SettingsNavigationHandler>(),
        ];

        foreach (NavigationHandler handler in handlers)
            await sidebarRegistry.RegisterAsync(handler);

        foreach (NavigationHandler handler in fixedHandlers)
            await fixedSidebarRegistry.RegisterFixedAsync(handler);
    }
}