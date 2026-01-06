using System;
using System.Globalization;
using System.Windows.Data;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters.Arithmetics;

public class NumberCapConverter : IValueConverter
{
    public required double Maximum { get; init; }
    public string Format { get; init; } = "{0}";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            double v = Converter.ToDouble(value);
            if (Maximum < v)
                return string.Format(culture, Format, Maximum) + "+";

            return string.Format(culture, Format, v);
        }
        catch
        {
            return "";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}