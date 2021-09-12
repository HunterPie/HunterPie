using HunterPie.UI.Controls.Buttons;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal
{
    internal class BooleanVisualConverter : IVisualConverter
    {
        public UIElement Build(object parent, PropertyInfo childInfo)
        {
            var binding = VisualConverterHelper.CreateBinding(parent, childInfo.Name);
            Switch @switch = new()
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };
            
            BindingOperations.SetBinding(@switch, Switch.IsActiveProperty, binding);
            
            return @switch;
        }
    }
}
