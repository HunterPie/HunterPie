using HunterPie.UI.Client.Sidebar.Handler;
using System.Collections.ObjectModel;

namespace HunterPie.UI.SideBar.Services;

public interface ISideBarCollection
{
    public ObservableCollection<INavigationHandler> Handlers { get; }
}