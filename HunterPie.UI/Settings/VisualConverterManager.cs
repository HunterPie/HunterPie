using HunterPie.Core.Logger;
using HunterPie.Core.Settings;
using HunterPie.UI.Controls.Settings;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Settings.Converter;
using HunterPie.UI.Settings.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Settings
{
    public class VisualConverterManager
    {
        private static VisualConverterManager _instance;
        private static VisualConverterManager Instance
        {
            get 
            {
                if (_instance is null)
                    _instance = new();

                return _instance;
            }
        }

        private readonly Dictionary<Type, IVisualConverter> _converters = new Dictionary<Type, IVisualConverter>()
        {
            { typeof(bool), new BooleanVisualConverter() },
            { typeof(string), new StringVisualConverter() },
            { typeof(IFileSelector), new FileSelectorVisualConverter() }
        };

        private VisualConverterManager() {}

        public static ISettingElement[] Build(object settings)
        {
            Log.Benchmark();
            List<SettingElementViewModel> holder = new List<SettingElementViewModel>();

            Type parentType = settings.GetType();

            foreach (PropertyInfo property in parentType.GetProperties())
            {
                Type type = property.PropertyType;
                if (type.GetInterfaces().Contains(typeof(ISettings)))
                {
                    StackPanel panel = new();
                    object parent = property.GetValue(settings);
                    BuildChildren(parent, panel);

                    SettingsGroup metadata = (SettingsGroup)Attribute.GetCustomAttribute(type, typeof(SettingsGroup));

                    SettingElementViewModel vm = new(metadata.Name, metadata.Description, metadata.Icon, panel);

                    holder.Add(vm);
                }
            }
            Log.BenchmarkEnd();
            return holder.ToArray();
        }

        public static void AddConverterFor<T>(IVisualConverter converter)
        {
            if (Instance._converters.ContainsKey(typeof(T)))
                return;

            Instance._converters.Add(typeof(T), converter);
        }

        public static UIElement ConvertElement(object parent, PropertyInfo childInfo)
        {
            Type type;

            foreach (Type @interface in childInfo.PropertyType.GetInterfaces())
            {
                if (Instance._converters.ContainsKey(@interface))
                    break;
            }

            type = childInfo.PropertyType;

            if (!Instance._converters.ContainsKey(type))
                return null;

            return Instance._converters[type].Build(parent, childInfo);
        }

        private static void BuildChildren(object parent, Panel panel)
        {
            Type parentType = parent.GetType();

            foreach (PropertyInfo prop in parentType.GetProperties())
            {
                Type type = prop.PropertyType;
                object value = prop.GetValue(parent);
                
                UIElement element = ConvertElement(parent, prop);
                SettingField metadata = prop.GetCustomAttribute<SettingField>();
                SettingElementHost settingHost = new()
                {
                    Hosted = element,
                    Text = metadata.Name,
                    Description = metadata.Description
                };

                panel.Children.Add(settingHost);
            }
        }
    }
}
