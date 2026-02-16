using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

#nullable enable
public class LocalizationResolver : IValueConverter
{
    private static ILocalizationRepository Repository => DependencyContainer.Get<ILocalizationRepository>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not string xpath
            ? "Not localizable"
            : Repository.FindStringBy(xpath);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}