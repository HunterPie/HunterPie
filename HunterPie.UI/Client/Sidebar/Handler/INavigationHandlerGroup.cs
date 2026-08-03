using System.Collections.Generic;

namespace HunterPie.UI.Client.Sidebar.Handler;

public interface INavigationHandlerGroup : ILabeledNavigation
{
    public IReadOnlyCollection<ILabeledNavigation> Navigations { get; }
}
