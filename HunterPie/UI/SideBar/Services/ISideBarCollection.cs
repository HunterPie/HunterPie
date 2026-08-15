using HunterPie.UI.Client.Sidebar.Handler;
using System.Collections.Generic;

namespace HunterPie.UI.SideBar.Services;

public interface ISideBarCollection
{
    public IReadOnlyCollection<NavigationHandler> Handlers { get; }

    public IReadOnlyCollection<NavigationHandler> FixedHandlers { get; }
}