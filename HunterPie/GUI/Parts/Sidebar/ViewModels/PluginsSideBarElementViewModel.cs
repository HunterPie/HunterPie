using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class PluginsSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Application.Current.FindResource("ICON_PLUGIN") as ImageSource;

        public string Text => "Plugins";

        public bool IsActivable => true;

        public void ExecuteOnClick()
        {

        }
    }
}
