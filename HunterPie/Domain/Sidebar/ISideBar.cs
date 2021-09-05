using HunterPie.GUI.Parts.Sidebar.ViewModels;

namespace HunterPie.Domain.Sidebar
{
    public interface ISideBar
    {
        public ISideBarElement[] Menu { get; }
    }
}
