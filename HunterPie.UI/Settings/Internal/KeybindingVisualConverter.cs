using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using KeybindingControl = HunterPie.UI.Controls.Buttons.Keybinding;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal
{
    public class KeybindingVisualConverter : IVisualConverter
    {
        public FrameworkElement Build(object parent, PropertyInfo childInfo)
        {
            Keybinding key = (Keybinding)childInfo.GetValue(parent);
            var binding = VisualConverterHelper.CreateBinding(key, nameof(Keybinding.KeyCombo));

            KeybindingControl control = new();

            BindingOperations.SetBinding(control, KeybindingControl.HotKeyProperty, binding);
            
            return control;
        }
    }
}
