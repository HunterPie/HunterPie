using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using System.Reflection;

namespace HunterPie.UI.Controls.Settings.ViewModel;

internal class SettingElementType : Bindable, ISettingElementType
{
    private bool _match = true;

    public object Parent { get; }
    public PropertyInfo Information { get; }
    public GameProcess? Game { get; }
    public string Name { get; }
    public string Description { get; }
    public bool RequiresRestart { get; }

    public bool Match
    {
        get => _match;
        set => SetValue(ref _match, value);
    }

    public SettingElementType(
        GameProcess? game,
        string name,
        string description,
        object parent,
        PropertyInfo info,
        bool restart
    )
    {
        Game = game;
        Name = name;
        Description = description;
        Parent = parent;
        Information = info;
        RequiresRestart = restart;
    }
}
