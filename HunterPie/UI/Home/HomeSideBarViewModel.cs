using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Process;
using HunterPie.Internal;
using HunterPie.UI.Architecture;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Home;

internal class HomeSideBarViewModel : ViewModel, ISideBarViewModel
{
    public Type Type => typeof(HomeViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='HOME_STRING']";

    public string Icon => "ICON_HOME";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public void Execute()
    {
        var supportedGames = new ObservableCollection<SupportedGameViewModel>
        {
            new()
            {
                Banner = "https://cdn.hunterpie.com/resources/monster-hunter-world-banner.png",
                Execute = () => Steam.RunGameBy(GameType.World),
                IsAvailable = true,
                Name = Games.MONSTER_HUNTER_WORLD,
                Status = ProcessStatus.Waiting
            },
            new()
            {
                Banner =
                    "https://cdn.hunterpie.com/resources/monster-hunter-rise-banner.png",
                Execute = () => Steam.RunGameBy(GameType.Rise),
                IsAvailable = true,
                Name = Games.MONSTER_HUNTER_RISE,
                Status = ProcessStatus.Waiting
            },
        };

        Navigator.Body.Navigate(new HomeViewModel(supportedGames));
    }
}