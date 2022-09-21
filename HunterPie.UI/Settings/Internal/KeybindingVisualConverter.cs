using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using KeybindingControl = HunterPie.UI.Controls.Buttons.Keybinding;

namespace HunterPie.UI.Settings.Internal;

public class KeybindingVisualConverter : IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var key = (Keybinding)childInfo.GetValue(parent);
        Binding binding = VisualConverterHelper.CreateBinding(key, nameof(Keybinding.KeyCombo));

        KeybindingControl control = new();

        _ = BindingOperations.SetBinding(control, KeybindingControl.HotKeyProperty, binding);

        return control;
    }
}
