using HunterPie.UI.Architecture.Converters.Brushes;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Markup;

#nullable enable
public class BrushColor : MarkupExtension
{
    private readonly BrushToColorConverter _converter = new();

    public required string Key { get; init; }

    public double Opacity { get; set; } = 1.0;

    public BrushColor(string key)
    {
        Key = key;
    }

    public override object? ProvideValue(IServiceProvider provider)
    {
        return _converter.Convert(
            value: Resources.TryGet<Brush>(Key),
            targetType: typeof(Color),
            parameter: Opacity,
            culture: CultureInfo.CurrentCulture
        );
    }
}