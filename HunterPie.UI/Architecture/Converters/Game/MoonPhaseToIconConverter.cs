using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Moon;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters.Game;

#nullable enable
public class MoonPhaseToIconConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MoonPhase phase)
            return null;

        string? iconName = phase switch
        {
            MoonPhase.Full => "Icons.Moon.Full",
            MoonPhase.WaningGibbous => "Icons.Moon.WaningGibbous",
            MoonPhase.ThirdQuarter => "Icons.Moon.ThirdQuarter",
            MoonPhase.WaningCrescent => "Icons.Moon.WaningCrescent",
            MoonPhase.WaxingCrescent => "Icons.Moon.WaxingCrescent",
            MoonPhase.FirstQuarter => "Icons.Moon.FirstQuarter",
            MoonPhase.WaxingGibbous => "Icons.Moon.WaxingGibbous",
            _ => null
        };

        if (iconName is null)
            return null;

        return Resources.Icon(iconName);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}