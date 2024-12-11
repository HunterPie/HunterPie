using HunterPie.Domain.Sidebar.Elements;
using HunterPie.UI.Home;

namespace HunterPie.Domain.Sidebar;

internal static class SideBarProvider
{
    public static ISideBarCollection SideBar = new SimpleSideBarBuilder()
        .WithButton(new HomeSideBarViewModel(new HomeService(), new HomeCallToActionsService()))
        .WithButton(new ConsoleSideBarViewModel())
        .WithButton(new SettingsSideBarViewModel())
        .WithButton(new PatchNotesSideBarViewModel())
        .WithButton(new QuestStatisticsSideBarViewModel())
        .WithButton(new DiscordSideBarViewModel())
        .WithButton(new PatreonSideBarViewModel())
        .WithButton(new GitHubSideBarViewModel())
        .Build();
}