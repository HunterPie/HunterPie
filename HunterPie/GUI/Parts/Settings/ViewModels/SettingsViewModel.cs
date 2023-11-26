using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Core.Search;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.Integrations.Poogie.Version.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.GUI.Parts.Settings.ViewModels;

#nullable enable
public class SettingsViewModel : ViewModel
{
    private readonly PoogieVersionConnector _connector = new();
    private int _currentTabIndex;
    private UpdateFetchStatus _updateStatus = UpdateFetchStatus.Fetching;
    private readonly Dictionary<GameProcess, ObservableCollection<ConfigurationCategory>> _configurations;
    private ObservableCollection<ConfigurationCategory> _categories;

    public ObservableCollection<GameProcess> ConfigurableGames { get; }
    public Observable<GameProcess> SelectedGameConfiguration { get; }

    public UpdateFetchStatus UpdateStatus { get => _updateStatus; set => SetValue(ref _updateStatus, value); }
    public ObservableCollection<ConfigurationCategory> Categories { get => _categories; set => SetValue(ref _categories, value); }
    public int CurrentTabIndex { get => _currentTabIndex; set => SetValue(ref _currentTabIndex, value); }

    public SettingsViewModel(
        Dictionary<GameProcess, ObservableCollection<ConfigurationCategory>> configurations,
        ObservableCollection<GameProcess> configurableGames,
        Observable<GameProcess> currentConfiguredGame
    )
    {
        _configurations = configurations;
        ConfigurableGames = configurableGames;
        SelectedGameConfiguration = currentConfiguredGame;
        _categories = _configurations[currentConfiguredGame.Value];
    }

    public async void FetchVersion()
    {
        UpdateStatus = UpdateFetchStatus.Fetching;
        PoogieResult<VersionResponse> response = await _connector.Latest();

        switch (response)
        {
            case { Error: { } }:
                UpdateStatus = UpdateFetchStatus.Error;
                return;
            case { Response: { } versionResponse }:
                var version = new Version(versionResponse.LatestVersion);
                UpdateStatus = ClientInfo.IsVersionGreaterOrEq(version)
                    ? UpdateFetchStatus.Latest
                    : UpdateFetchStatus.NeedsUpdate;
                break;
        }
    }

    public void Search(string query)
    {
        Categories.SelectMany(it => it.Groups.SelectMany(group => group.Properties))
            .ForEach(it =>
            {
                if (it is not ConfigurationPropertyViewModel vm)
                    return;

                vm.IsMatch = SearchEngine.IsMatch($"{vm.Name} {vm.Description}", query);
            });
    }

    public void ChangeSettingsGroup()
    {
        Categories = _configurations[SelectedGameConfiguration];
        CurrentTabIndex = 0;
    }
}