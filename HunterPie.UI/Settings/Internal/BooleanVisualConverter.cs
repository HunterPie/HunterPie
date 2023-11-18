using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Controls.Buttons;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal;

internal class BooleanVisualConverter : IVisualConverter
{
    public FrameworkElement Build(GameProcess? game, object parent, PropertyInfo childInfo)
    {
        var observable = (Observable<bool>)childInfo.GetValue(parent);
        Binding binding = VisualConverterHelper.CreateBinding(observable);
        Switch @switch = new()
        {
            HorizontalAlignment = HorizontalAlignment.Right
        };

        _ = BindingOperations.SetBinding(@switch, Switch.IsActiveProperty, binding);
        return @switch;
    }
}
