using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.UI.Assets.Application;

namespace HunterPie.UI.Architecture.Converters;

public class WirebugStateToColorConverter : IValueConverter
{
    private readonly Dictionary<WirebugState, Color> _stateColors = new(6)
    {
        { WirebugState.None, Resources.Get<Color>("WIREBUG_NORMAL") },
        { WirebugState.Blocked, Resources.Get<Color>("WIREBUG_NORMAL") },
        { WirebugState.IceBlight, Resources.Get<Color>("WIREBUG_ICEBLIGHT") },
        { WirebugState.WindMantle, Resources.Get<Color>("WIREBUG_WINDMANTLE") },
        { WirebugState.RubyWirebug, Resources.Get<Color>("WIREBUG_RUBY") },
        { WirebugState.GoldWirebug, Resources.Get<Color>("WIREBUG_GOLD") }
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is WirebugState wirebugState)
        {
            Color color = _stateColors[wirebugState];
            return color;
        }

        throw new ArgumentException("value must be wirebugState");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}