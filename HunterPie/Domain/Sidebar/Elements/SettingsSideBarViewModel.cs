using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Client.Events;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.Internal.Initializers;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.SideBar.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Domain.Sidebar.Elements;

internal class SettingsSideBarViewModel : ViewModel, ISideBarViewModel
{
    private SettingsViewModel? _viewModel;
    public Type Type => typeof(SettingsViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='SETTINGS_STRING']";

    public string Icon => "ICON_SETTINGS";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public SettingsSideBarViewModel()
    {
        ConfigManager.OnSync += OnConfigurationSync;
    }

    private void OnConfigurationSync(object? sender, ConfigSaveEventArgs e)
    {
        if (Path.GetFileNameWithoutExtension(e.Path) != "config")
            return;

        if (_viewModel is not { })
            return;

        _viewModel.SynchronizedAt = e.SyncedAt;
    }

    public void Execute()
    {
        BuildViewModel().ContinueWith(async (it) =>
        {
            _viewModel = await it;
            Navigator.Body.Navigate(_viewModel);
        });
    }

    private async Task<SettingsViewModel> BuildViewModel()
    {
        Observable<GameProcess> game = ClientConfig.Config.Client.LastConfiguredGame;

        ObservableCollection<ConfigurationCategory> generalConfig = ConfigurationAdapter.Adapt(ClientConfig.Config);
        ObservableCollection<ConfigurationCategory> accountConfig = await LocalAccountConfig.BuildAccountConfig();
        ObservableCollection<ConfigurationCategory> featureFlags = ClientConfig.Config.Client.EnableFeatureFlags.Value switch
        {
            true => FeatureFlagAdapter.Adapt(FeatureFlagsInitializer.Features.Flags),
            _ => new ObservableCollection<ConfigurationCategory>()
        };

        var commonConfig = accountConfig.Concat(generalConfig)
            .Concat(featureFlags)
            .ToObservableCollection();
        var configurations = new Dictionary<GameProcess, ObservableCollection<ConfigurationCategory>>
        {
            { GameProcess.MonsterHunterRise, BuildConfiguration(commonConfig, ClientConfig.Config.Rise, GameProcess.MonsterHunterRise) },
            { GameProcess.MonsterHunterWorld, BuildConfiguration(commonConfig, ClientConfig.Config.World, GameProcess.MonsterHunterWorld) }
        };
        var supportedConfigurations =
            new ObservableCollection<GameProcess>(new List<GameProcess>
            {
                GameProcess.MonsterHunterRise,
                GameProcess.MonsterHunterWorld
            });

        return new SettingsViewModel(configurations, supportedConfigurations, game);
    }

    private static ObservableCollection<ConfigurationCategory> BuildConfiguration(
        IEnumerable<ConfigurationCategory> commonConfiguration,
        GameConfig configuration,
        GameProcess gameProcess
    )
    {
        ObservableCollection<ConfigurationCategory> configCategory = ConfigurationAdapter.Adapt(configuration, gameProcess);

        return commonConfiguration.Concat(configCategory)
            .ToObservableCollection();
    }
}