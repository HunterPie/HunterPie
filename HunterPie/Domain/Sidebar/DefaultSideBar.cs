using HunterPie.GUI.Parts.Sidebar.ViewModels;

namespace HunterPie.Domain.Sidebar;

internal class DefaultSideBar : ISideBar
{
    public ISideBarElement[] Menu { get; } = new ISideBarElement[]
    {
        new ConsoleSideBarElementViewModel(),
        new SettingsSideBarElementViewModel(),
        new PluginsSideBarElementViewModel(),
        new PatchNotesSideBarElementViewModel(),
        new PatreonSideBarElementViewModel(),
        new DiscordSideBarElementViewModel(),
        new GithubSideBarElementViewModel(),
    };
}
