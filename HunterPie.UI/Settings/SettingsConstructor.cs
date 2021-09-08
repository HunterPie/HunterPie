using HunterPie.Core.Logger;
using HunterPie.Core.Settings;
using HunterPie.UI.Controls.Settings;
using HunterPie.UI.Controls.Settings.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Settings
{
    public class SettingsConstructor
    {
        public ISettingElement[] Build(object settings)
        {
            return BuildParent(settings);
        }

        private SettingElementViewModel[] BuildParent(object settings)
        {
            List<SettingElementViewModel> parent = new List<SettingElementViewModel>();
            
            Type parentType = settings.GetType();

            foreach (var property in parentType.GetProperties())
            {
                Type propType = property.PropertyType;
                if (propType.GetInterfaces().Contains(typeof(ISettings)))
                {
                    Stopwatch timer = Stopwatch.StartNew();
                    Log.Debug($"========== BUILDING PARENT ===========");
                    
                    StackPanel panel = new StackPanel();
                    object propValue = property.GetValue(settings);
                    
                    BuildChildren(propValue, ref panel);

                    var attribs = GetAttributes(propType);
                    Log.Debug($"Parent metadata: {attribs.Name}, {attribs.Description}, {attribs.Icon}");
                    SettingElementViewModel vm = new(attribs.Name, attribs.Description, attribs.Icon, panel);

                    parent.Add(vm);
                    
                    Log.Debug($"Time taken: {timer.ElapsedMilliseconds:0.00}ms");
                    timer.Stop();
                    Log.Debug($"========== FINISHED PARENT ===========");
                }
            }

            return parent.ToArray();
        }

        private void BuildChildren(object parent, ref StackPanel panel)
        {
            Type parentType = parent.GetType();

            foreach (PropertyInfo property in parentType.GetProperties())
            {
                Type propType = property.PropertyType;
                object value = property.GetValue(parent);
                SettingElementHost visualModel = SettingsConstructorHelper.BuildHostByType(value);
                SettingField metadata = GetSettingField(property);
                
                visualModel.Text = metadata.Name;
                visualModel.Description = metadata.Description;
                
                Log.Debug($"Building ['{property.Name}', type=[{property.PropertyType.Name}], UIElement=[{visualModel.GetType().Name}] ]");

                panel.Children.Add(visualModel);
            }

        }

        private SettingsGroup GetAttributes(Type type)
        {
            SettingsGroup attrib = (SettingsGroup)Attribute.GetCustomAttribute(type, typeof(SettingsGroup));

            return attrib;
        }

        private SettingField GetSettingField(PropertyInfo info)
        {
            return info.GetCustomAttribute<SettingField>();
        }
    }
}
