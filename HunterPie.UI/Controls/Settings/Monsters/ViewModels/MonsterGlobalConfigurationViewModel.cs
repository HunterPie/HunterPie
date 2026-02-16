using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

#nullable enable
public class MonsterGlobalConfigurationViewModel(
    ObservableCollection<MonsterPartGroupViewModel> parts,
    ObservableCollection<MonsterGlobalAilmentViewModel> ailments
    ) : ViewModel
{
    public ObservableCollection<MonsterPartGroupViewModel> Parts { get; } = parts;

    public ObservableCollection<MonsterGlobalAilmentViewModel> Ailments { get; } = ailments;
    public bool IsExpanded { get; set => SetValue(ref field, value); }
}