using HunterPie.Core.Logger;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters
{
    public class CurrentValueToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            
            double currentValue = (double)values[0];
            double maxValue = (double)values[1];
            double maxWidth = (double)values[2];
            Thickness border = (Thickness)values[3];
            double sides = border.Left + border.Right;

            double width = maxWidth * (currentValue / maxValue) - sides;
            return Math.Max(1.0, width);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
