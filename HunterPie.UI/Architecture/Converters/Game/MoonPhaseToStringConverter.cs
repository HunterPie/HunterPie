using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Moon;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters.Game;

#nullable enable
public class MoonPhaseToStringConverter : IValueConverter
{
    private static ILocalizationRepository LocalizationRepository =>
        DependencyContainer.Get<ILocalizationRepository>();

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MoonPhase phase)
            return null;

        string? id = phase switch
        {
            MoonPhase.Full => "MOON_FULL",
            MoonPhase.WaningGibbous => "MOON_WANING_GIBBOUS",
            MoonPhase.ThirdQuarter => "MOON_THIRD_QUARTER",
            MoonPhase.WaningCrescent => "MOON_WANING_CRESCENT",
            MoonPhase.WaxingCrescent => "MOON_WAXING_CRESCENT",
            MoonPhase.FirstQuarter => "MOON_FIRST_QUARTER",
            MoonPhase.WaxingGibbous => "MOON_WAXING_GIBBOUS",
            _ => null
        };

        if (id is null)
            return null;

        return LocalizationRepository.FindStringBy($"//Strings/Client/Overlay/String[@Id='{id}']");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}