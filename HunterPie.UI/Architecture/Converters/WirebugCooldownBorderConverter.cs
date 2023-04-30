using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class WirebugCooldownBorderConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 4)
            return 0.0;

        if (values[0] is not double
            || values[1] is not double)
        {
            return 0.0;
        }

        object isAvailable = values[2];
        object onCooldown = values[3];

        if (isAvailable is true && onCooldown is false)
            return 1.0;

        double cooldown = (double)values[0];
        double maxCooldown = (double)values[1];

        return 1 - (cooldown / maxCooldown);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
