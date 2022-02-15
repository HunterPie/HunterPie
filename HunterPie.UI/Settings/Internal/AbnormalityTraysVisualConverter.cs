using HunterPie.Core.Settings.Types;
using HunterPie.UI.Controls.Settings.Custom;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
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
