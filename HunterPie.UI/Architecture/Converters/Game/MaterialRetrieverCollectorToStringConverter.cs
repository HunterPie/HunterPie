using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters.Game;

public class MaterialRetrieverCollectorToStringConverter : IValueConverter
{
    private ILocalizationRepository LocalizationRepository => DependencyContainer.Get<ILocalizationRepository>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MaterialRetrievalCollector collector)
            return null;

        return LocalizationRepository.FindStringBy(
            $"//Strings/Activities/Wilds/MaterialRetrieval/Collector[@Id='{collector.ToString().ToUpperInvariant()}']"
        );
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}