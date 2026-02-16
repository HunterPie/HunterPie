using HunterPie.Domain.Sidebar;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.SideBar.ViewModels;

internal class SideBarViewModel(
    ISideBarCollection collection
    ) : ViewModel
{
    public ObservableCollection<ISideBarViewModel> Elements { get; } = collection.Elements;
}