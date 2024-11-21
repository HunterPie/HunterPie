using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.System;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.Internal;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Home;

internal class HomeService
{

    private static readonly Dictionary<GameType, SupportedGameViewModel> SupportedGames = new()
    {
        {
            GameType.World,
            new SupportedGameViewModel
            {
                Banner = "https://cdn.hunterpie.com/resources/monster-hunter-world-banner.png",
                Execute = () => Steam.RunGameBy(GameType.World),
                Name = Games.MONSTER_HUNTER_WORLD,
                Status = ProcessStatus.Waiting,
                OnSettings = BuildSettingsHandler(GameProcess.MonsterHunterWorld)
            }
        },
        {
            GameType.Rise, new SupportedGameViewModel
            {
                Banner =
                    "https://cdn.hunterpie.com/resources/monster-hunter-rise-banner.png",
                Execute = () => Steam.RunGameBy(GameType.Rise),
                Name = Games.MONSTER_HUNTER_RISE,
                Status = ProcessStatus.Waiting,
                OnSettings = BuildSettingsHandler(GameProcess.MonsterHunterRise)
            }
        }
    };

    public void Subscribe()
    {
        foreach (IProcessManager manager in ProcessManager.Managers)
        {
            GameType? possibleGameType = MapFactory.Map<GameProcess, GameType?>(manager.Game);

            if (possibleGameType is not { } gameType)
                continue;

            SupportedGameViewModel? supportedGame = SupportedGames.GetValueOrDefault(gameType);

            if (supportedGame is not { })
                continue;

            manager.OnProcessStatusChange += (_, args) => supportedGame.Status = args.NewValue;
        }
    }

    public ObservableCollection<SupportedGameViewModel> GetSupportedGameViewModels() =>
        SupportedGames.Values.ToObservableCollection();

    private static Action BuildSettingsHandler(GameProcess gameProcess)
    {
        return () =>
        {
            GameConfig gameConfig = ClientConfigHelper.GetGameConfigBy(gameProcess);

            ObservableCollection<ConfigurationCategory> generalConfig = ConfigurationAdapter.Adapt(ClientConfig.Config);
            ObservableCollection<ConfigurationCategory> gameCategories =
                ConfigurationAdapter.Adapt(gameConfig, gameProcess);

            var configurationCategories = generalConfig.Concat(gameCategories)
                .ToObservableCollection();
            var configurations = new Dictionary<GameProcess, ObservableCollection<ConfigurationCategory>>()
            {
                { gameProcess, configurationCategories }
            };
            var supportedConfigurations = new[] { gameProcess }.ToObservableCollection();

            var settingsViewModel = new SettingsViewModel(configurations, supportedConfigurations, gameProcess);

            Navigator.Body.Navigate(settingsViewModel);
        };
    }
}