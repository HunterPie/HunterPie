using HunterPie.UI.Assets.Application;
using HunterPie.UI.Settings.Converter;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal;

internal class EnumVisualConverter : IVisualConverter
{
    public DataTemplate EnumElementDataTemplate { get; } = Resources.Get<DataTemplate>("DATA_TEMPLATE_SETTINGS_ENUM_ELEMENT");

    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        object observable = childInfo.GetValue(parent);
        Binding binding = VisualConverterHelper.CreateBinding(observable);

        ObservableCollection<object> elements = new();

        foreach (object value in Enum.GetValues(childInfo.PropertyType.GenericTypeArguments[0]))
            elements.Add(value);

        ComboBox box = new()
        {
            ItemsSource = elements,
            ItemTemplate = EnumElementDataTemplate,
        };
        _ = BindingOperations.SetBinding(box, ComboBox.SelectedItemProperty, binding);

        return box;
    }
}
