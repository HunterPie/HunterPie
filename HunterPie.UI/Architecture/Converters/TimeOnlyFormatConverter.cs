using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class TimeOnlyFormatConverter : IMultiValueConverter
{
    public string Format24 { get; set; } = "HH:mm";
    public string Format12 { get; set; } = "hh:mm tt";

    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is not [TimeOnly time, bool use24])
            return Binding.DoNothing;

        string format = use24 ? Format24 : Format12;

        return use24
            ? time.ToString(format)
            : time.ToString(format, culture);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}