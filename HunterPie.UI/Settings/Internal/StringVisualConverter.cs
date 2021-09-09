using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal
{
    internal class StringVisualConverter : IVisualConverter
    {
        public UIElement Build(object parent, PropertyInfo childInfo)
        {
            var binding = VisualConverterHelper.CreateBinding(parent, childInfo.Name);
            TextBox textbox = new();

            BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);

            return textbox;
        }
    }
}
