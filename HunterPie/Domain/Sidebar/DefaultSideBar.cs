using HunterPie.GUI.Parts.Sidebar.ViewModels;

namespace HunterPie.Domain.Sidebar
{
    internal class DefaultSideBar : ISideBar
    {
        private readonly ISideBarElement[] _menu = new ISideBarElement[]
        {
            new ConsoleSideBarElementViewModel(),
            new SettingsSideBarElementViewModel(),
            new PluginsSideBarElementViewModel(),
            new PatchNotesSideBarElementViewModel(),
            new PatreonSideBarElementViewModel(),
            new DiscordSideBarElementViewModel()
        };

        public ISideBarElement[] Menu => _menu;
    }
}
