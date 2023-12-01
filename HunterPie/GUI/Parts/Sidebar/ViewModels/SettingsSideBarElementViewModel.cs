using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Client.Events;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.GUI.Parts.Settings.Views;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class SettingsSideBarElementViewModel : ISideBarElement
{
    private SettingsViewModel? _viewModel;
    public ImageSource Icon => Resources.Icon("ICON_SETTINGS");

    public string Text => Localization.QueryString("//Strings/Client/Tabs/Tab[@Id='SETTINGS_STRING']");

    public bool IsActivable => true;

    public bool IsEnabled => true;

    public bool ShouldNotify => false;

    public SettingsSideBarElementViewModel()
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

    public void ExecuteOnClick()
    {
        Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            Observable<GameProcess> game = ClientConfig.Config.Client.LastConfiguredGame;

            ObservableCollection<ConfigurationCategory> generalConfig = ConfigurationAdapter.Adapt(ClientConfig.Config);
            ObservableCollection<ConfigurationCategory> accountConfig = await LocalAccountConfig.BuildAccountConfig();

            var commonConfig = accountConfig.Concat(generalConfig)
                .ToObservableCollection();
            var configurations = new Dictionary<GameProcess, ObservableCollection<ConfigurationCategory>>
            {
                { GameProcess.MonsterHunterRise, BuildConfiguration(commonConfig, ClientConfig.Config.Rise, GameProcess.MonsterHunterRise) },
                { GameProcess.MonsterHunterWorld, BuildConfiguration(commonConfig, ClientConfig.Config.World, GameProcess.MonsterHunterWorld) },
                { GameProcess.MonsterHunterRiseSunbreakDemo, BuildConfiguration(commonConfig, ClientConfig.Config.Rise, GameProcess.MonsterHunterRiseSunbreakDemo) }
            };
            var supportedConfigurations =
                new ObservableCollection<GameProcess>(new List<GameProcess>
                {
                    GameProcess.MonsterHunterRise,
                    GameProcess.MonsterHunterWorld,
                    GameProcess.MonsterHunterRiseSunbreakDemo
                });

            _viewModel = new SettingsViewModel(configurations, supportedConfigurations, game);
            var host = new SettingsView
            {
                DataContext = _viewModel
            };

            Navigator.Navigate(host, true);
        });
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
