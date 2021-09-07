using HunterPie.Core.Settings;
using HunterPie.UI.Controls.Buttons;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Settings
{
    internal class SettingsConstructorHelper
    {
        private Dictionary<Type, Type> _equivalents = new Dictionary<Type, Type>()
        {
            { typeof(bool), typeof(Switch) },
            { typeof(string), typeof(TextBox) },
            { typeof(double), typeof(Slider) },
            { typeof(ISettings), typeof(StackPanel) }
        };

        public UIElement GetElementByType<T>(T property)
        {
            Type type = typeof(T);
            
            UIElement instance = (UIElement)Activator.CreateInstance(type);

            return instance;
        }
    }
}
