using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
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
            
            Binding binding = VisualConverterHelper.CreateBinding(child, nameof(IFileSelector.Current));

            ComboBox ui = new()
            {
                ItemsSource = child.Elements
            };

            BindingOperations.SetBinding(ui, ComboBox.SelectedItemProperty, binding);
            return ui;
        }
    }
}
