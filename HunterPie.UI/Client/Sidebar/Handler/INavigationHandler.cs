using HunterPie.UI.Client.Sidebar.Entity;
using System.Threading.Tasks;

namespace HunterPie.UI.Client.Sidebar.Handler;

public interface INavigationHandler : ILabeledNavigation
{
    public SideBarButtonState State { get; set; }

    public bool IsActive { get; set; }

    public Task InitializeAsync();

    public Task ExecuteAsync();
}