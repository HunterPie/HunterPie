using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Models;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Settings.ViewModels;

public class SettingsViewModel : ViewModel
{
    private int _currentTabIndex;

    public int CurrentTabIndex { get => _currentTabIndex; set => SetValue(ref _currentTabIndex, value); }
    public ObservableCollection<ConfigurationCategory> Categories { get; }

    public SettingsViewModel(ObservableCollection<ConfigurationCategory> categories)
    {
        Categories = categories;
    }
}