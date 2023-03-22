using HunterPie.GUI.Parts.Sidebar.ViewModels;
using System.Linq;

namespace HunterPie.Domain.Sidebar;

#nullable enable
internal static class SideBar
{
    public static ISideBarElement[] Menu { get; } = new ISideBarElement[]
    {
        new ConsoleSideBarElementViewModel(),
        new SettingsSideBarElementViewModel(),
        new PatchNotesSideBarElementViewModel(),
        new QuestStatisticsSideBarElementViewModel(),
        //new PluginsSideBarElementViewModel(),
        new PatreonSideBarElementViewModel(),
        new DiscordSideBarElementViewModel(),
        new GithubSideBarElementViewModel(),
        //new DebugSideBarElementViewModel(),
    };

    public static T? GetInstance<T>() => (T?)Menu.FirstOrDefault(vm => vm is T);
}
