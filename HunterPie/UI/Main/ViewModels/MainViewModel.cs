using HunterPie.UI.Architecture;
using HunterPie.UI.Header.ViewModels;
using HunterPie.UI.SideBar.ViewModels;

namespace HunterPie.UI.Main.ViewModels;

internal class MainViewModel : ViewModel
{
    public HeaderViewModel HeaderViewModel { get; init; }
    public SideBarViewModel SideBarViewModel { get; init; }

    private ViewModel? _navigationViewModel;
    public ViewModel? NavigationViewModel { get => _navigationViewModel; set => SetValue(ref _navigationViewModel, value); }

    public MainViewModel(
        HeaderViewModel headerViewModel,
        SideBarViewModel sideBarViewModel
    )
    {
        HeaderViewModel = headerViewModel;
        SideBarViewModel = sideBarViewModel;
    }
}