using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal;

internal class StringVisualConverter : IVisualConverter
{
    public FrameworkElement Build(GameProcess? game, object parent, PropertyInfo childInfo)
    {
        var observable = (Observable<string>)childInfo.GetValue(parent);
        Binding binding = VisualConverterHelper.CreateBinding(observable);
        TextBox textbox = new();

        _ = BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);

        return textbox;
    }
}
