using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class AbnormalityNameConverter : IValueConverter
{
    private static ILocalizationRepository Repository => DependencyContainer.Get<ILocalizationRepository>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Repository.FindStringBy($"//Strings/Abnormalities/Abnormality[@Id='{value}']");

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}