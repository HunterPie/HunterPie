using HunterPie.Core.Logger;
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
        public FrameworkElement Build(object parent, PropertyInfo childInfo)
        {
            IFileSelector child = (IFileSelector)childInfo.GetValue(parent);
            
            Binding binding = VisualConverterHelper.CreateBinding(child, nameof(IFileSelector.Current));

            ComboBox ui = new()
            {
                ItemsSource = child.Elements
            };

            BindingOperations.SetBinding(ui, ComboBox.SelectedValueProperty, binding);
            return ui;
        }
    }
}
