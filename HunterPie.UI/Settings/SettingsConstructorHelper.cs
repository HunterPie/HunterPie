using HunterPie.Core.Settings;
using HunterPie.UI.Controls.Buttons;
using HunterPie.UI.Controls.Settings;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Settings
{
    internal static class SettingsConstructorHelper
    {
        private static readonly Dictionary<Type, Type> _equivalents = new Dictionary<Type, Type>()
        {
            { typeof(bool), typeof(Switch) },
            { typeof(string), typeof(TextBox) },
            { typeof(double), typeof(Slider) },
        };

        public static SettingElementHost BuildHostByType(object property)
        {
            Type type = property.GetType();
            
            if (property is ISettings)
                return null;

            if (property is IFileSelector) 
                return BuildFileSelector(property as IFileSelector);

            Type elementType = _equivalents[type];

            UIElement instance = (UIElement)Activator.CreateInstance(elementType);

            SettingElementHost host = new SettingElementHost()
            {
                Hosted = instance
            };

            return host;
        }

        public static SettingElementHost BuildFileSelector(IFileSelector selector)
        {
            ComboBox box = new ComboBox();

            foreach (object obj in selector.List())
                box.Items.Add(obj);

            SettingElementHost host = new SettingElementHost()
            {
                Hosted = box
            };

            return host;
        }
    }
}
