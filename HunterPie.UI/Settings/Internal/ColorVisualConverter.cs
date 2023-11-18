using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Controls.Buttons;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;

namespace HunterPie.UI.Settings.Internal;

public class ColorVisualConverter : IVisualConverter
{
    public FrameworkElement Build(GameProcess? game, object parent, PropertyInfo childInfo)
    {
        var viewModel = (Color)childInfo.GetValue(parent);
        return new ColorPicker()
        {
            DataContext = viewModel,
        };
    }
}
