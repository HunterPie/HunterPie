using HunterPie.Core.Logger;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Settings.Converter;
using HunterPie.UI.Settings.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Range = HunterPie.Core.Settings.Types.Range;

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
            { typeof(Range), new RangeVisualConverter() },
            { typeof(Secret), new SecretVisualConverter() },
            { typeof(IFileSelector), new FileSelectorVisualConverter() },
            { typeof(Enum), new EnumVisualConverter() },
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
                    SettingsGroup metadata = (SettingsGroup)Attribute.GetCustomAttribute(type, typeof(SettingsGroup));
                    SettingElementViewModel vm = new(metadata.Name, metadata.Description, metadata.Icon);

                    object parent = property.GetValue(settings);
                    BuildChildren(parent, vm);

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
            // In case of interfaces we can still convert property
            foreach (Type @interface in childInfo.PropertyType.GetInterfaces())
            {
                if (Instance._converters.ContainsKey(@interface))
                    return Instance._converters[@interface].Build(parent, childInfo);
            }

            Type type = childInfo.PropertyType;

            if (type.IsGenericType)
                type = type.GenericTypeArguments.FirstOrDefault();

            if (type.IsEnum)
                return Instance._converters[typeof(Enum)].Build(parent, childInfo);

            if (!Instance._converters.ContainsKey(type))
                return null;

            return Instance._converters[type].Build(parent, childInfo);
        }

        private static void BuildChildren(object parent, ISettingElement panel)
        {
            Type parentType = parent.GetType();

            foreach (PropertyInfo prop in parentType.GetProperties())
            {
                SettingField metadata = prop.GetCustomAttribute<SettingField>();
                
                if (prop.PropertyType.GetInterfaces().Contains(typeof(IWidgetSettings)))
                {
                    BuildChildren(prop.GetValue(parent), panel);
                } else
                {
                    SettingElementType settingHost = new(
                        name: metadata.Name,
                        description: metadata.Description,
                        parent,
                        prop
                    );
                    panel.Add(settingHost);
                }
            }
        }
    }
}
