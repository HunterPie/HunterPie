using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;
public class PercentageToRelativeSizeConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 3)
            throw new ArgumentException("there must be 3 double values");

        double current = (double)values[0];
        double max = Math.Max(1, (double)values[1]);
        double relativeSize = (double)values[2];

        return current / max * relativeSize;
    }
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
