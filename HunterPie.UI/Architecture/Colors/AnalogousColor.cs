using System.Windows.Media;

namespace HunterPie.UI.Architecture.Colors;

public static class AnalogousColor
{
    public static Color NegativeFrom(Color main, double angle)
    {
        var hsl = HslColor.FromColor(main);

        hsl.Hue = (hsl.Hue - angle) % 360;

        return hsl.ToColor();
    }
}