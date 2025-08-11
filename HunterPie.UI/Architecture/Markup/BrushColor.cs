using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Markup;

#nullable enable
public class BrushColor : MarkupExtension
{
    public required Brush Brush { get; init; }

    public double Opacity { get; set; } = 1.0;

    public override object? ProvideValue(IServiceProvider _)
    {
        Color? color = Brush.Clone() switch
        {
            SolidColorBrush solid => solid.Color,
            _ => null,
        };

        if (color is not { } clr)
            return null;

        clr.A = (byte)(255 * Opacity);

        return clr;
    }
}