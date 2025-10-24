using HunterPie.UI.Architecture.Converters.Brushes;
using System;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Markup;

#nullable enable
[MarkupExtensionReturnType(typeof(Color))]
public class BrushColor : MarkupExtension
{
    private readonly BrushToColorConverter _converter = new();

    public required SolidColorBrush Brush { get; init; }

    public double Opacity { get; set; } = 1.0;

    public override object? ProvideValue(IServiceProvider provider)
    {
        return _converter.Convert(
            value: Brush,
            targetType: typeof(Color),
            parameter: Opacity,
            culture: CultureInfo.CurrentCulture
        );
    }
}