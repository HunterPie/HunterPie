using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Damage.Presentation;

#nullable enable
/// <summary>
/// Converter that turns a Hex color string into the main accent color for the Damage Widget v2
/// </summary>
public class PlayerDamageBarGradientBrushConverter : IValueConverter
{
    private readonly Color _transparent = Color.FromArgb(0, 0, 0, 0);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string val)
            return null;

        var color = (Color)ColorConverter.ConvertFromString(val);

        var collection = new GradientStopCollection
        {
            new GradientStop(_transparent, 0),
            new GradientStop(color, 1),
        };
        collection.Freeze();

        var brush = new LinearGradientBrush
        {
            GradientStops = collection,
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1.2, 0)
        };

        brush.Freeze();

        return brush;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}