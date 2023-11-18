using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Adapters;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Settings.Converter;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal;

#nullable enable
internal class EnumVisualConverter : IVisualConverter
{
    public DataTemplate EnumElementDataTemplate { get; } = Resources.Get<DataTemplate>("DATA_TEMPLATE_SETTINGS_ENUM_ELEMENT");

    public FrameworkElement Build(GameProcess? game, object parent, PropertyInfo childInfo)
    {
        ObservableCollection<object> elements = new();
        object observable = childInfo.GetValue(parent)!;
        Binding binding = VisualConverterHelper.CreateBinding(observable);

        SettingAdapter? adapterAttribute = childInfo.GetCustomAttribute<SettingAdapter>();
        object[] enumValues = (game, adapterAttribute) switch
        {
            ({ } gameType, { Adapter: IEnumAdapter adapter }) => adapter.GetValues(gameType),
            _ => Enum.GetValues(childInfo.PropertyType.GenericTypeArguments[0])
                     .Cast<object>()
                     .ToArray(),
        };

        foreach (object value in enumValues)
            elements.Add(value);

        ComboBox box = new()
        {
            ItemsSource = elements,
            ItemTemplate = EnumElementDataTemplate,
            MinHeight = 35,
            VerticalAlignment = VerticalAlignment.Center
        };
        _ = BindingOperations.SetBinding(box, ComboBox.SelectedItemProperty, binding);

        return box;
    }
}
