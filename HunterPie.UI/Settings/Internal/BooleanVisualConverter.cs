using HunterPie.Core.Architecture;
using HunterPie.UI.Controls.Buttons;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal
{
    internal class BooleanVisualConverter : IVisualConverter
    {
        public FrameworkElement Build(object parent, PropertyInfo childInfo)
        {
            Observable<bool> observable = (Observable<bool>)childInfo.GetValue(parent);
            var binding = VisualConverterHelper.CreateBinding(observable);
            Switch @switch = new()
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };

            BindingOperations.SetBinding(@switch, Switch.IsActiveProperty, binding);
            return @switch;
        }
    }
}
