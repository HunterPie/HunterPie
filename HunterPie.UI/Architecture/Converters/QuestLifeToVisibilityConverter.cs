using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters;

public class QuestLifeToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int questlife = Converter.ToInt16(value);
        return questlife == 0
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
