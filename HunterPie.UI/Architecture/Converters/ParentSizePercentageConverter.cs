using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters
{
    public class ParentSizePercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Check for errors
            // TODO: Make parameter the percentage divisor
            if (values[0] is not double ||
                values[1] is not double)
                return 0;

            double parentSize = (double)values[0];
            double percentage = (double)values[1] / 100;

            return parentSize * percentage;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
