using HunterPie.DI;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Patches.Navigation;
using HunterPie.Features.Settings.Navigation;
using HunterPie.Features.Statistics.Navigation;
using HunterPie.Features.Theme.Navigation;
using HunterPie.Integrations.Discord.Navigation;
using HunterPie.Integrations.Github.Navigation;
using HunterPie.Integrations.Patreon.Navigation;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Client.Sidebar.Service;
using HunterPie.UI.Home.Navigation;
using HunterPie.UI.Logging.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class SideBarInitializer(
    IDependencyRegistry dependencies,
    ISideBarRegistry sidebarRegistry
) : IInitializer
{
    public async Task Init()
    {
        INavigationHandler[] handlers = [
            dependencies.Get<HomeNavigationHandler>(),
            dependencies.Get<ConsoleNavigationHandler>(),
            dependencies.Get<SettingsNavigationHandler>(),
            dependencies.Get<ThemeNavigationHandler>(),
            dependencies.Get<PatchNotesNavigationHandler>(),
            dependencies.Get<QuestStatisticsNavigationHandler>(),
            dependencies.Get<DiscordNavigationHandler>(),
            dependencies.Get<PatreonNavigationHandler>(),
            dependencies.Get<GitHubNavigationHandler>()
        ];


        foreach (INavigationHandler handler in handlers)
            await sidebarRegistry.RegisterAsync(handler);
    }
}