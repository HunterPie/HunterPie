using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Settings.Factory;

internal class SettingsFactory(
    PoogieVersionConnector versionConnector,
    LocalAccountConfig localAccountConfig,
    DefaultFeatureFlags defaultFeatureFlags,
    ConfigurationAdapter configurationAdapter,
    FeatureFlagAdapter featureFlagAdapter
)
{
    public async Task<SettingsViewModel> CreateFullAsync(Observable<GameProcessType> currentGame)
    {
        ConfigurationCategoryGroup[] commonConfigurations = await BuildCommonConfigurationAsync();

        var configurations = new Dictionary<GameProcessType, ObservableCollection<ConfigurationCategoryGroup>>
        {
            { GameProcessType.MonsterHunterRise, BuildGameConfiguration(commonConfigurations, ClientConfig.Config.Rise, GameProcessType.MonsterHunterRise) },
            { GameProcessType.MonsterHunterWorld, BuildGameConfiguration(commonConfigurations, ClientConfig.Config.World, GameProcessType.MonsterHunterWorld) },
            { GameProcessType.MonsterHunterWilds, BuildGameConfiguration(commonConfigurations, ClientConfig.Config.Wilds, GameProcessType.MonsterHunterWilds ) }
        };
        var supportedConfigurations =
            new ObservableCollection<GameProcessType>(
            [
                GameProcessType.MonsterHunterRise,
                GameProcessType.MonsterHunterWorld,
                GameProcessType.MonsterHunterWilds
            ]);

        return new SettingsViewModel(
            configurations: configurations,
            configurableGames: supportedConfigurations,
            currentConfiguredGame: currentGame,
            connector: versionConnector
        );
    }

    public async Task<SettingsViewModel> CreatePartialAsync(GameProcessType game)
    {
        GameConfig config = ClientConfigHelper.GetGameConfigBy(game);
        ConfigurationCategoryGroup[] commonConfigurations = await BuildCommonConfigurationAsync();

        var configurations = new Dictionary<GameProcessType, ObservableCollection<ConfigurationCategoryGroup>>
        {
            { game, BuildGameConfiguration(commonConfigurations, config, game) }
        };
        ObservableCollection<GameProcessType> supportedConfigurations = new[] { game }.ToObservableCollection();

        return new SettingsViewModel(
            configurations: configurations,
            configurableGames: supportedConfigurations,
            currentConfiguredGame: game,
            connector: versionConnector
        );
    }

    private ObservableCollection<ConfigurationCategoryGroup> BuildGameConfiguration(
        IEnumerable<ConfigurationCategoryGroup> commonConfiguration,
        GameConfig configuration,
        GameProcessType gameProcessType
    )
    {
        ObservableCollection<ConfigurationCategoryGroup> configCategory = configurationAdapter.Adapt(configuration, gameProcessType);

        return commonConfiguration
            .Concat(configCategory)
            .GroupBy(it => it.Name)
            .Select(it =>
                new ConfigurationCategoryGroup(
                    Name: it.Key,
                    Categories: it.SelectMany(group => group.Categories)
                        .ToObservableCollection()
                )
            )
            .ToObservableCollection();
    }

    private async Task<ConfigurationCategoryGroup[]> BuildCommonConfigurationAsync()
    {
        ObservableCollection<ConfigurationCategoryGroup> generalConfig = configurationAdapter.Adapt(ClientConfig.Config);
        ObservableCollection<ConfigurationCategoryGroup> accountConfig = await localAccountConfig.BuildAccountConfigAsync();
        ObservableCollection<ConfigurationCategoryGroup> featureFlags = ClientConfig.Config.Client.EnableFeatureFlags.Value switch
        {
            true => featureFlagAdapter.Adapt(defaultFeatureFlags.Flags),
            _ => new ObservableCollection<ConfigurationCategoryGroup>()
        };

        return accountConfig
            .Concat(generalConfig)
            .Concat(featureFlags)
            .ToArray();
    }
}