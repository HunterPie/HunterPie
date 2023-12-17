using HunterPie.UI.Navigation;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Main.Presentation;

internal class NavigationDataTemplateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            null => null,
            { } => NavigationProvider.FindBy(value.GetType())
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}