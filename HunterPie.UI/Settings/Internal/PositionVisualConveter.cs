using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Controls.TextBox;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;

namespace HunterPie.UI.Settings.Internal;

internal class PositionVisualConveter : IVisualConverter
{
    public FrameworkElement Build(GameProcess? game, object parent, PropertyInfo childInfo)
    {
        return new PositionTextBox()
        {
            DataContext = childInfo.GetValue(parent),
            MaxWidth = 200
        };
    }
}
