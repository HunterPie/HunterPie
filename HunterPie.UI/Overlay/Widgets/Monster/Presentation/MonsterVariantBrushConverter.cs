using HunterPie.Core.Game.Entity.Enemy;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Monster.Presentation;

#nullable enable
public class MonsterVariantBrushConverter : IValueConverter
{
    private static readonly Point StartPoint = new(1, 1);
    private static readonly Point EndPoint = new(0.4, 0.4);

    private static readonly Color NormalColor = Color.FromRgb(0x3A, 0x3E, 0x40);
    private static readonly Color TemperedColor = Color.FromRgb(0x6C, 0x24, 0xB4);
    private static readonly Color FrenzyColor = Color.FromRgb(0xFA, 0x02, 0x7E);
    private static readonly Color ArchTemperedColor = Color.FromRgb(0xFF, 0x7A, 0x05);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not VariantType variant)
            return null;

        Color headerColor = variant.HasFlag(VariantType.ArchTempered)
            ? ArchTemperedColor
            : variant.HasFlag(VariantType.Tempered)
                ? TemperedColor
                : NormalColor;

        Color bottomColor = variant.HasFlag(VariantType.Frenzy)
            ? FrenzyColor
            : headerColor;

        var collection = new GradientStopCollection
        {
            new GradientStop(headerColor, 1),
            new GradientStop(bottomColor, 0)
        };

        collection.Freeze();

        var brush = new LinearGradientBrush()
        {
            StartPoint = StartPoint,
            EndPoint = EndPoint,
            GradientStops = collection
        };

        brush.Freeze();

        return brush;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}