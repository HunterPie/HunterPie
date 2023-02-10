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
        string iconName = null;

        switch ((WirebugState)value)
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
                return Resources.Icon("ICON_WIREBUG");
        }

        return iconName is null ? Resources.Icon("ICON_WIREBUG") : Resources.Icon(iconName);
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}