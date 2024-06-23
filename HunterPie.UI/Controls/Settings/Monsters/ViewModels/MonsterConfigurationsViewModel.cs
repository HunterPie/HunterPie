using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterConfigurationsViewModel : ViewModel, IDisposable
{
    private readonly MonsterDetails _configuration;
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