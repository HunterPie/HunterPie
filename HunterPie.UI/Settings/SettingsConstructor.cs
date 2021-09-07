using HunterPie.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Settings
{
    public class SettingsConstructor
    {

        public UIElement Build(ISettings settings)
        {
            StackPanel parent = new StackPanel();

            return parent;
        }

    }
}
