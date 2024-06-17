using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterConfigurationsViewModel : ViewModel
{

    public ObservableCollection<MonsterConfigurationViewModel> Elements { get; }

    public MonsterConfigurationsViewModel(ObservableCollection<MonsterConfigurationViewModel> elements)
    {
        Elements = elements;
    }
}