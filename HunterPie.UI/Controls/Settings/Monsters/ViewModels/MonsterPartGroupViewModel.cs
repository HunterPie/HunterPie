using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

#nullable enable
public class MonsterPartGroupViewModel(
    PartGroupType type,
    ObservableHashSet<PartGroupType> groups
    ) : ViewModel
{
    private readonly ObservableHashSet<PartGroupType> _groups = groups;

    public PartGroupType Type { get; } = type;
    public bool IsEnabled { get; set => SetValue(ref field, value); }

    public void Toggle()
    {
        IsEnabled = !IsEnabled;

        if (IsEnabled)
            _groups.Add(Type);
        else
            _groups.Remove(Type);
    }
}