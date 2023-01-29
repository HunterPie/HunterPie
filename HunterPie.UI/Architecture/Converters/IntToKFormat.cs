using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class IntToKFormat : IValueConverter
{
    private readonly char[] _suffixes = { 'K', 'M', 'B', 'T', 'Q' };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int integer)
            throw new ArgumentException("value must be an integer");

        double @decimal = (double)integer;
        int kSuffix = -1;
        bool isOverThousand = integer >= 1_000;
        while (integer >= 1_000)
        {
            integer /= 1_000;
            @decimal /= 1_000.0;
            kSuffix++;
        }

        return isOverThousand ? $"{@decimal:0.0}{_suffixes[kSuffix]}" : integer;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}