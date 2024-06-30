using HunterPie.Core.Search;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterConfigurationsViewModel : ViewModel, IDisposable
{
    private readonly MonsterDetails _configuration;

    private bool _isSearching;
    public bool IsSearching { get => _isSearching; set => SetValue(ref _isSearching, value); }

    public ObservableCollection<MonsterConfigurationViewModel> Elements { get; }

    public MonsterConfigurationsViewModel(
        MonsterDetails configuration,
        ObservableCollection<MonsterConfigurationViewModel> elements
    )
    {
        _configuration = configuration;
        Elements = elements;
    }

    public void BindAndUpdateSettings()
    {
        foreach (MonsterConfigurationViewModel viewModel in Elements)
            viewModel.PropertyChanged += OnViewModelPropertyChange;
    }

    public void Dispose()
    {
        foreach (MonsterConfigurationViewModel viewModel in Elements)
            viewModel.PropertyChanged -= OnViewModelPropertyChange;
    }

    public void FilterQuery(string query)
    {
        foreach (MonsterConfigurationViewModel viewModel in Elements)
            viewModel.IsMatch = string.IsNullOrEmpty(query) || SearchEngine.IsMatch(viewModel.Name, query);
    }

    private void OnViewModelPropertyChange(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(MonsterConfigurationViewModel.IsOverriding))
            return;

        if (sender is not MonsterConfigurationViewModel vm)
            return;

        if (vm.IsOverriding)
            _configuration.Monsters.Add(vm.Configuration);
        else
            _configuration.Monsters.Remove(vm.Configuration);
    }
}