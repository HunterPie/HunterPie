using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class WirebugStateToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not WirebugState state)
            throw new ArgumentException($"Argument must be of type {nameof(WirebugState)} but got {value?.GetType()}");

        string iconName = "ICON_WIREBUG";

        switch (state)
        {
            case WirebugState.IceBlight:
                iconName = "ICON_WIREBUG_ICEBLIGHT";
                break;
            case WirebugState.WindMantle:
                iconName = "ICON_WIREBUG_WINDMANTLE";
                break;
            case WirebugState.RubyWirebug:
                iconName = "ICON_WIREBUG_RUBY";
                break;
            case WirebugState.GoldWirebug:
                iconName = "ICON_WIREBUG_GOLD";
                break;
            default:
                return Resources.Icon(iconName);
        }

        return Resources.Icon(iconName);
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}