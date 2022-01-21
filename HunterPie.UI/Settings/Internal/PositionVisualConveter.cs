using HunterPie.UI.Controls.TextBox;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;

namespace HunterPie.UI.Settings.Internal
{
    class PositionVisualConveter : IVisualConverter
    {
        public FrameworkElement Build(object parent, PropertyInfo childInfo)
        {
            return new PositionTextBox()
            {
                DataContext = childInfo.GetValue(parent),
                MaxWidth = 200
            };
        }
    }
}
