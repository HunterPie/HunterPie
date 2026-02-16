using HunterPie.Core.Extensions;
using HunterPie.Core.Search;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterConfigurationsViewModel(
    MonsterGlobalConfigurationViewModel globalConfiguration,
    MonsterDetailsConfiguration configuration,
    MonsterConfigurationViewModel[] elements,
    IBodyNavigator bodyNavigator
) : ViewModel
{
    private readonly MonsterDetailsConfiguration _configuration = configuration;
    private readonly MonsterConfigurationViewModel[] _elements = elements;

    public bool IsSearching { get; set => SetValue(ref field, value); }

    public ObservableCollection<MonsterConfigurationViewModel> Overrides { get; } = elements.Where(it => it.IsOverriding)
            .ToObservableCollection();

    public ObservableCollection<MonsterConfigurationViewModel> SearchElements { get; } = elements.Where(it => !it.IsOverriding)
            .ToObservableCollection();

    public MonsterGlobalConfigurationViewModel GlobalConfiguration { get; } = globalConfiguration;

    public void FetchIcons()
    {
        foreach (MonsterConfigurationViewModel viewModel in _elements)
            viewModel.FetchIcon();
    }

    public void FilterQuery(string query)
    {
        foreach (MonsterConfigurationViewModel viewModel in SearchElements)
            viewModel.IsMatch = string.IsNullOrEmpty(query) || SearchEngine.IsMatch(viewModel.Name, query);
    }

    public void CreateOverride(MonsterConfigurationViewModel viewModel)
    {
        viewModel.IsOverriding = true;
        Overrides.Add(viewModel);
        SearchElements.Remove(viewModel);
        _configuration.Monsters.Add(viewModel.Configuration);
    }

    public void RemoveOverride(MonsterConfigurationViewModel viewModel)
    {
        viewModel.IsOverriding = false;
        Overrides.Remove(viewModel);
        SearchElements.Add(viewModel);
        _configuration.Monsters.Remove(viewModel.Configuration);
    }

    public void Return() => bodyNavigator.Return();
}