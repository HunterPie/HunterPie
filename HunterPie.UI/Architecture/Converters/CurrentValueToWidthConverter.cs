using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters
{
    public class CurrentValueToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double currentValue = Converter.ToDouble(values[0]);
            double maxValue = Converter.ToDouble(values[1]);
            double maxWidth = Converter.ToDouble(values[2]);
            Thickness border = (Thickness)values[3];
            double sides = border.Left + border.Right;

            double width = maxWidth * (currentValue / Math.Max(maxValue, 1)) - sides;
            return Math.Max(1.0, width);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
