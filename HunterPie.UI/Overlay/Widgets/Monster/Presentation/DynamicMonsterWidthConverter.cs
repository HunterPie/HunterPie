using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Overlay.Widgets.Monster.Presentation;

public class DynamicMonsterWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is not bool isEnabled)
            return double.NaN;

        if (values[1] is not double minSize)
            return double.NaN;

        if (values[2] is not double maxSize)
            return double.NaN;

        if (values[3] is not int monstersCount)
            return double.NaN;

        if (!isEnabled)
            return maxSize;

        return minSize + ((3 - monstersCount) * (minSize * 0.25));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}