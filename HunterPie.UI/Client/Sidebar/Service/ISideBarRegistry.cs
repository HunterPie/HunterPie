using HunterPie.UI.Client.Sidebar.Handler;
using System.Threading.Tasks;

namespace HunterPie.UI.Client.Sidebar.Service;

public interface ISideBarRegistry
{
    public Task RegisterAsync(NavigationHandler handler);
}