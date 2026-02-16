using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class MonsterPartIdToNameConverter : IValueConverter
{
    private static ILocalizationRepository Repository => DependencyContainer.Get<ILocalizationRepository>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string id = (string)value;
        string xPath = $"//Strings/Monsters/Shared/Part[@Id='{id}']";

        return Repository.ExistsBy(xPath)
            ? Repository.FindStringBy(xPath)
            : id;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}