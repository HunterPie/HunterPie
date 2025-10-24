using HunterPie.DI;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Service;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Overlay.Presentation.Converters;

#nullable enable
[ValueConversion(typeof(ViewModel), typeof(DataTemplate))]
public class WidgetByViewModelConverter : IValueConverter
{
    private IWidgetProvider Provider => DependencyContainer.Get<IWidgetProvider>();

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ViewModel vm)
            return null;

        return Provider.Provide(vm);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}