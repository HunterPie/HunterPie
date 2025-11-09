using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class RiseMonsterAilmentIdToNameConverter : IValueConverter
{
    private static ILocalizationRepository Repository => DependencyContainer.Get<ILocalizationRepository>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (DesignModeViewModels.IsDesignMode)
            return "Ailment";

        string path = $"//Strings/Ailments/Rise/Ailment[@Id='{value}']";

        return Repository.ExistsBy(path)
               ? Repository.FindStringBy(path)
               : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}