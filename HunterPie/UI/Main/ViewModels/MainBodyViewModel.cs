using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Internal;
using HunterPie.UI.Architecture;
using HunterPie.UI.SideBar.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Main.ViewModels;

internal class MainBodyViewModel : ViewModel
{
    public SideBarViewModel SideBarViewModel { get; init; }

    private ViewModel? _navigationViewModel;
    public ViewModel? NavigationViewModel { get => _navigationViewModel; set => SetValue(ref _navigationViewModel, value); }

    public Observable<GameType> SelectedGame => ClientConfig.Config.Client.DefaultGameType;

    public ObservableCollection<GameType> Games { get; } = new() { GameType.Rise, GameType.World };

    public MainBodyViewModel(SideBarViewModel sideBarViewModel)
    {
        SideBarViewModel = sideBarViewModel;
    }

    public void LaunchGame()
    {
        Steam.RunGameBy(SelectedGame.Value);
    }
}