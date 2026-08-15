using HunterPie.UI.Architecture;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.SideBar.Services;
using System.Collections.Generic;

namespace HunterPie.UI.SideBar.ViewModels;

internal class SideBarViewModel(
    ISideBarCollection collection
) : ViewModel
{
    public IReadOnlyCollection<NavigationHandler> Elements { get; } = collection.Handlers;

    public IReadOnlyCollection<NavigationHandler> FixedElements { get; } = collection.FixedHandlers;
}