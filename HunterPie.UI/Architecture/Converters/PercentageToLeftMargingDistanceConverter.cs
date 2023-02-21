﻿using HunterPie.UI.Architecture.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class PercentageToLeftMargingDistanceConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double width = (double)values[0];
        var oldThickness = parameter.ToThickness();

        if (double.IsNaN(width))
            return oldThickness;

        double percentage = (double)values[1];

        double left = width * percentage;

        if (double.IsNaN(left) || double.IsInfinity(left))
            return oldThickness;

        return oldThickness with { Left = left };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
