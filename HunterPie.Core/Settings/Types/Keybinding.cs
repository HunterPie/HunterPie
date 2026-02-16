using HunterPie.Core.Architecture;

namespace HunterPie.Core.Settings.Types;

public class Keybinding : Bindable
{
    public string KeyCombo
    {
        get;
        set => SetValue(ref field, value);
    }

    public static implicit operator string(Keybinding keybinding) => keybinding.KeyCombo;
    public static implicit operator Keybinding(string keybinding) => new() { KeyCombo = keybinding };
}