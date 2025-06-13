using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using PrimitiveColor = System.Windows.Media.Colors;

namespace HunterPie.UI.Architecture.Converters.Brushes;

public class BrushToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush solidBrush)
            return PrimitiveColor.Transparent;

        return solidBrush.Color;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}