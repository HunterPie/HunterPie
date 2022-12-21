using HunterPie.Core.Game.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class CombatStatusToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (CombatStatus)value == CombatStatus.None
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
