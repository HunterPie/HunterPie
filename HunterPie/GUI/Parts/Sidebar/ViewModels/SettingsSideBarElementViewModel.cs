using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Generics;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.GUI.Parts.Settings.Views;
using HunterPie.Internal.Initializers;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Controls.Flags;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class SettingsSideBarElementViewModel : ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_SETTINGS");

    public string Text => Localization.QueryString("//Strings/Client/Tabs/Tab[@Id='SETTINGS_STRING']");

    public bool IsActivable => true;

    public bool IsEnabled => true;

    public bool ShouldNotify => false;

    public SettingsSideBarElementViewModel()
    {
        RefreshWindowOnChange(ClientConfig.Config.Client.LastConfiguredGame);
        RefreshWindowOnChange(ClientConfig.Config.Client.EnableFeatureFlags);
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

            var host = new SettingsView
            {
                DataContext = new SettingsViewModel(configurations, supportedConfigurations, game)
            };
            Navigator.Navigate(host, true);
        });
    }

    private ObservableCollection<ConfigurationCategory> BuildConfiguration(
        IEnumerable<ConfigurationCategory> commonConfiguration,
        GameConfig configuration,
        GameProcess gameProcess
    )
    {
        ObservableCollection<ConfigurationCategory> configCategory = ConfigurationAdapter.Adapt(configuration, gameProcess);

        return commonConfiguration.Concat(configCategory)
            .ToObservableCollection();
    }

    private async void RefreshSettingsWindow(bool forceRefresh = false)
    {
        await Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            ISettingElement[] settingTabs = VisualConverterManager.Build(ClientConfig.Config);

            ISettingElement[] gameSpecificTabs = VisualConverterManager.Build(
                ClientConfigHelper.GetGameConfigBy(ClientConfig.Config.Client.LastConfiguredGame.Value)
            );

            ISettingElement[] accountConfig = await LocalAccountConfig.CreateAccountSettingsTab();

            accountConfig = accountConfig.Concat(settingTabs)
                .Concat(gameSpecificTabs)
                .ToArray();

            GenericFileSelector _ = ClientConfig.Config.Client.Language;

            SettingHostViewModel vm = new(accountConfig);
            var host = new SettingHost() { DataContext = vm };

            // Also add feature flags if enabled
            if (ClientConfig.Config.Client.EnableFeatureFlags)
                vm.Elements.Add(new FeatureFlagsView(FeatureFlagsInitializer.Features.Flags));

            Navigator.Navigate(host, forceRefresh);
        });
    }

    private void RefreshWindowOnChange(Bindable observable) => observable.PropertyChanged += (_, __) =>
    {
        if (!Navigator.IsInstanceOf<SettingHost>())
            return;

        RefreshSettingsWindow(true);
    };

}
