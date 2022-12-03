using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class SecondsToTimeString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string timeFormat = "mm\\:ss";
        if (value is double val)
            try
            {
                var span = TimeSpan.FromSeconds(val);

                if (parameter is string format)
                    timeFormat = format;

                return span.ToString(timeFormat);
            }
            catch
            {
                return string.Empty;
            }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
