using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters;

public class SharpnessToPreviousColorConverter : IValueConverter
{

    private readonly Dictionary<Sharpness, Color> _associatedColors = new Dictionary<Sharpness, Color>(7)
    {
        { Sharpness.Broken, Resources.Get<Color>("Colors.Widgets.Sharpness.Broken") },
        { Sharpness.Red, Resources.Get<Color>("Colors.Widgets.Sharpness.Red") },
        { Sharpness.Orange, Resources.Get<Color>("Colors.Widgets.Sharpness.Orange") },
        { Sharpness.Yellow, Resources.Get<Color>("Colors.Widgets.Sharpness.Yellow") },
        { Sharpness.Green, Resources.Get<Color>("Colors.Widgets.Sharpness.Green") },
        { Sharpness.Blue, Resources.Get<Color>("Colors.Widgets.Sharpness.Blue") },
        { Sharpness.White, Resources.Get<Color>("Colors.Widgets.Sharpness.White") },
        { Sharpness.Purple, Resources.Get<Color>("Colors.Widgets.Sharpness.Purple") },
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Sharpness sharpness && parameter is bool shouldConvert)
        {
            Color color = _associatedColors[sharpness - 1];

            return shouldConvert ? new SolidColorBrush(color) : color;
        }

        throw new ArgumentException("value must be sharpness");
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}