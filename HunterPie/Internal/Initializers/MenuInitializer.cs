using HunterPie.Domain.Interfaces;
using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using HunterPie.GUI.Parts.Sidebar.Service;
using System.Linq;

namespace HunterPie.Internal.Initializers;

internal class MenuInitializer : IInitializer
{
    public void Init()
    {
        SideBarContainer.SetMenu(SideBar.Menu);

        SideBarService.Navigate(SideBar.Menu.FirstOrDefault());
    }
}
