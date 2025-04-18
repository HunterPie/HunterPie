using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

#nullable enable
public class EnumToStringConverter : IValueConverter
{
    private ILocalizationRepository LocalizationRepository => DependencyContainer.Get<ILocalizationRepository>();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.GetType().IsEnum != true
            ? null
            : LocalizationRepository.FindByEnum(value).String;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}