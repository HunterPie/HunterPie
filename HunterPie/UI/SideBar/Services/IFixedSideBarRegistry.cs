using HunterPie.UI.Client.Sidebar.Handler;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.Services;

internal interface IFixedSideBarRegistry
{
    public Task RegisterFixedAsync(NavigationHandler handler);
}
