using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Resolvers;

public class BooleanResolver : IValueConverter
{
    public required object Truthy { get; init; }
    public required object Falsy { get; init; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool v)
            return Falsy;

        return v ? Truthy : Falsy;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}