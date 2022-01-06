using HunterPie.Core.Settings.Types;
using HunterPie.UI.Controls.TextBox;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal
{
    internal class SecretVisualConverter : IVisualConverter
    {
        public FrameworkElement Build(object parent, PropertyInfo childInfo)
        {
            Secret observable = (Secret)childInfo.GetValue(parent);
            var binding = VisualConverterHelper.CreateBinding(observable);
            SecretTextBox textbox = new();

            BindingOperations.SetBinding(textbox, SecretTextBox.TextProperty, binding);

            return textbox;
        }
    }
}
