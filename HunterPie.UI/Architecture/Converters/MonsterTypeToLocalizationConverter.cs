using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class MonsterTypeToLocalizationConverter : IValueConverter
{
    private static ILocalizationRepository Repository => DependencyContainer.Get<ILocalizationRepository>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string id)
            return Repository.FindStringBy($"//Strings/Monsters/Types/Type[@Id='{id}']");

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}