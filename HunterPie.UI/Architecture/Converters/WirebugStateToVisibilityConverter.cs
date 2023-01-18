using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class WirebugStateToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var wirebugState = (WirebugState)value;
        return wirebugState == WirebugState.Blocked
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}