using HunterPie.Core.Architecture;
using HunterPie.UI.Architecture.Converters;
using HunterPie.UI.Settings.Converter;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal
{
    internal class EnumVisualConverter : IVisualConverter
    {
        public FrameworkElement Build(object parent, PropertyInfo childInfo)
        {
            object observable = childInfo.GetValue(parent);
            var binding = VisualConverterHelper.CreateBinding(observable);

            binding.Converter = new EnumToStringConverter();

            ObservableCollection<string> elements = new();

            foreach (string enumName in Enum.GetNames(childInfo.PropertyType.GenericTypeArguments[0]))
                elements.Add(enumName);

            ComboBox box = new()
            {
                ItemsSource = elements 
            };

            BindingOperations.SetBinding(box, ComboBox.SelectedItemProperty, binding);

            return box;
        }
    }
}
