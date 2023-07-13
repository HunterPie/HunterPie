using HunterPie.UI.Architecture.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

#nullable enable
public class PercentageToLeftMarginDistanceConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        Thickness oldThickness = parameter?.ToThickness() ?? new Thickness();

        if (values[0] is not double width)
            return oldThickness;

        if (double.IsNaN(width))
            return oldThickness;

        double percentage = (double)values[1];

        double left = width * percentage;

        if (double.IsNaN(left) || double.IsInfinity(left))
            return oldThickness;

        return oldThickness with { Left = left };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
