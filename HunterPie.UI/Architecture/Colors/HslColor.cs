using System;
using System.Windows.Media;

namespace HunterPie.UI.Architecture;

public struct HslColor
{
    public double Hue;
    public double Saturation;
    public double Light;
    public byte Alpha;

    public Color ToColor()
    {
        static double HueToRgb(
            double v1,
            double v2,
            double vH)
        {
            if (vH < 0.0)
                vH += 1.0;
            if (vH > 1.0)
                vH -= 1.0;
            if (6.0 * vH < 1.0)
                return v1 + ((v2 - v1) * 6.0 * vH);
            if (2.0 * vH < 1.0)
                return v2;
            if (3.0 * vH < 2.0)
                return v1 + ((v2 - v1) * ((2.0 / 3.0) - vH) * 6.0);

            return v1;
        }

        double red, green, blue;

        double h = Hue / 360.0;
        double s = Saturation / 100.0;
        double l = Light / 100.0;

        if (Math.Abs(s - 0.0) < 0.00001)
        {
            red = l;
            green = l;
            blue = l;
        }
        else
        {
            double var2 = l switch
            {
                < 0.5 => l * (1.0 + s),
                _ => l + s - (s * l)
            };

            double var1 = (2.0 * l) - var2;

            red = HueToRgb(var1, var2, h + (1.0 / 3.0));
            green = HueToRgb(var1, var2, h);
            blue = HueToRgb(var1, var2, h - (1.0 / 3.0));
        }

        // --

        byte nRed = Convert.ToByte(red * 255.0);
        byte nGreen = Convert.ToByte(green * 255.0);
        byte nBlue = Convert.ToByte(blue * 255.0);

        return Color.FromArgb(Alpha, nRed, nGreen, nBlue);
    }

    public static HslColor FromColor(Color rgb)
    {
        double varR = rgb.R / 255.0;
        double varG = rgb.G / 255.0;
        double varB = rgb.B / 255.0;

        double varMin = Math.Min(Math.Min(varR, varG), varB);
        double varMax = Math.Max(Math.Max(varR, varG), varB);
        double delMax = varMax - varMin;

        double h;
        double s;
        double l = (varMax + varMin) / 2;

        if (Math.Abs(delMax - 0) < 0.00001)
        {
            h = 0;
            s = 0;
        }
        else
        {
            s = l switch
            {
                < 0.5 => delMax / (varMax + varMin),
                _ => delMax / (2.0 - varMax - varMin)
            };

            double delR = (((varMax - varR) / 6.0) + (delMax / 2.0)) / delMax;
            double delG = (((varMax - varG) / 6.0) + (delMax / 2.0)) / delMax;
            double delB = (((varMax - varB) / 6.0) + (delMax / 2.0)) / delMax;

            if (Math.Abs(varR - varMax) < 0.00001)
                h = delB - delG;
            else if (Math.Abs(varG - varMax) < 0.00001)
                h = (1.0 / 3.0) + delR - delB;
            else if (Math.Abs(varB - varMax) < 0.00001)
                h = (2.0 / 3.0) + delG - delR;
            else
                h = 0.0;

            if (h < 0.0)
                h += 1.0;

            if (h > 1.0)
                h -= 1.0;
        }

        return new HslColor
        {
            Hue = h * 360.0,
            Saturation = s * 100.0,
            Light = l * 100.0,
            Alpha = rgb.A
        };
    }
}