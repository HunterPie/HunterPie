using HunterPie.Domain.Interfaces;
using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using HunterPie.GUI.Parts.Sidebar.Service;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class MenuInitializer : IInitializer
{
    public Task Init()
    {
        SideBarContainer.SetMenu(SideBar.Menu);

        SideBarService.Navigate(SideBar.Menu.FirstOrDefault());

        return Task.CompletedTask;
    }
}
