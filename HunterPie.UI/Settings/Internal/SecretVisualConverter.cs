using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Controls.TextBox;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal;

internal class SecretVisualConverter : IVisualConverter
{
    public FrameworkElement Build(GameProcess? game, object parent, PropertyInfo childInfo)
    {
        var observable = (Secret)childInfo.GetValue(parent);
        Binding binding = VisualConverterHelper.CreateBinding(observable);
        SecretTextBox textbox = new();

        _ = BindingOperations.SetBinding(textbox, SecretTextBox.TextProperty, binding);

        return textbox;
    }
}
