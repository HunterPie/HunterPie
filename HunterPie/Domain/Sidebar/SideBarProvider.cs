using HunterPie.Domain.Sidebar.Elements;

namespace HunterPie.Domain.Sidebar;

internal static class SideBarProvider
{
    public static ISideBarCollection SideBar = new SimpleSideBarBuilder()
        .WithButton(new SettingsSideBarViewModel())
        .WithButton(new PatchNotesSideBarViewModel())
        .WithButton(new DiscordSideBarViewModel())
        .WithButton(new PatreonSideBarViewModel())
        .WithButton(new GitHubSideBarViewModel())
        .Build();
}