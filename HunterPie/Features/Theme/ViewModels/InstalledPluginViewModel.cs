using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Plugins.Entity;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledPluginViewModel(
    Plugin plugin,
    ConfigurationAdapter configAdapter,
    IBodyNavigator navigator,
    PoogieVersionConnector connector
) : ViewModel
{

    public string Name => plugin.Manifest.Name;

    public string Description => plugin.Manifest.Description;

    public string Version => $"v{plugin.Manifest.Version}";

    public string Author => plugin.Manifest.Author;

    public Observable<bool> IsEnabled => plugin.Configuration.IsEnabled;

    public void NavigateToSettings()
    {
        ObservableCollection<ConfigurationCategoryGroup> settings = configAdapter.Adapt(configuration: plugin.Configuration);

        var configurations = new Dictionary<GameProcessType, ObservableCollection<ConfigurationCategoryGroup>>
        {
            { GameProcessType.None, settings }
        };

        var viewModel = new SettingsViewModel(
            configurations: configurations,
            configurableGames: [],
            currentConfiguredGame: GameProcessType.None,
            connector: connector
        );

        navigator.Navigate(viewModel);
    }
}