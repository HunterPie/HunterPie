using HunterPie.Core.Architecture;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using System.Linq;

namespace HunterPie.UI.Controls.Settings.Monsters.Builders;

public static class MonsterGlobalConfigurationViewModelBuilder
{
    public static MonsterGlobalConfigurationViewModel Build(ObservableHashSet<PartGroupType> partGroups)
    {
        return new MonsterGlobalConfigurationViewModel(
            parts: partGroups.Select(it => new MonsterPartGroupViewModel(it, partGroups)
            {
                IsEnabled = partGroups.Contains(it)
            }).ToObservableCollection()
        );
    }
}