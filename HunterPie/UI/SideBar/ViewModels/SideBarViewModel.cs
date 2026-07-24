using HunterPie.UI.Architecture;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.SideBar.Services;
using System.Collections.ObjectModel;

namespace HunterPie.UI.SideBar.ViewModels;

internal class SideBarViewModel(
    ISideBarCollection collection
) : ViewModel
{
    public ObservableCollection<INavigationHandler> Elements { get; } = collection.Handlers;
}