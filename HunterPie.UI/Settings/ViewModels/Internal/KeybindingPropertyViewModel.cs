using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class KeybindingPropertyViewModel(Keybinding keyBinding) : ConfigurationPropertyViewModel
{
    public Keybinding KeyBinding { get; } = keyBinding;
}