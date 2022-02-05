using HunterPie.Core.Settings.Types;
using HunterPie.UI.Controls.Settings.Custom;
using HunterPie.UI.Settings.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.UI.Settings.Internal
{
    public class AbnormalityTraysVisualConverter : IVisualConverter
    {
        public FrameworkElement Build(object parent, PropertyInfo childInfo)
        {
            AbnormalityTrays viewModel = (AbnormalityTrays)childInfo.GetValue(parent);
            return new AbnormalityTrayList()
            {
                DataContext = viewModel
            };
        }
    }
}
