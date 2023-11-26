using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Core.Search;
using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.GUI.Parts.Settings.ViewModels;

public class SettingsViewModel : ViewModel
{
    private int _currentTabIndex;
    private readonly Dictionary<GameProcess, ObservableCollection<ConfigurationCategory>> _configurations;
    private ObservableCollection<ConfigurationCategory> _categories;


    public ObservableCollection<GameProcess> ConfigurableGames { get; }
    public Observable<GameProcess> SelectedGameConfiguration { get; }

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
    }
}