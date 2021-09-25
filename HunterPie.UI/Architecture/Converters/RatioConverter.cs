using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters
{
    public class RatioConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double a = (double)values[0];
            double b = (double)values[1];
            return a / b;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
