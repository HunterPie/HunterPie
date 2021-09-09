using HunterPie.Core.Settings;
using HunterPie.UI.Settings.Converter;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal
{
    internal class FileSelectorVisualConverter : IVisualConverter
    {
        public UIElement Build(object parent, PropertyInfo childInfo)
        {
            IFileSelector child = (IFileSelector)childInfo.GetValue(parent);
            Type selectorType = child.GetType();

            Binding binding = VisualConverterHelper.CreateBinding(
                child, 
                selectorType.GetProperty("Current").Name
            );

            ComboBox ui = new()
            {
                ItemsSource = child.Elements
            };

            BindingOperations.SetBinding(ui, ComboBox.SelectedItemProperty, binding);

            return ui;
        }
    }
}
