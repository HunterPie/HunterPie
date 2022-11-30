using HunterPie.UI.Architecture.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class PercentageToLeftMargingDistanceConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double width = (double)values[0];
        var oldThickness = parameter.ToThickness();

        if (double.IsNaN(width))
            return oldThickness;

        double percentage = (double)values[1];
        oldThickness.Left = width * percentage;

        return oldThickness;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
