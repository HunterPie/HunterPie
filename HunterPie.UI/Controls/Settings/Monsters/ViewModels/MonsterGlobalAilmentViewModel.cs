using HunterPie.Core.Architecture;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterGlobalAilmentViewModel(
    ObservableHashSet<int> allowedAilments,
    int id,
    string name
    ) : ViewModel
{
    private readonly ObservableHashSet<int> _allowedAilments = allowedAilments;

    private readonly int _id = id;

    public string Name { get; } = name;
    public bool IsEnabled { get; set => SetValue(ref field, value); }

    public void Toggle()
    {
        IsEnabled = !IsEnabled;

        if (IsEnabled)
            _allowedAilments.Add(_id);
        else
            _allowedAilments.Remove(_id);
    }
}