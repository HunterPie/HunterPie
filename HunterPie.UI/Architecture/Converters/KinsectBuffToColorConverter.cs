using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;


namespace HunterPie.UI.Architecture.Converters;

public class KinsectBuffToColorConverter : IValueConverter
{
    private static readonly KeyValuePair<KinsectBuff, Color>[] KinsectBuffColors =
    {
        new(KinsectBuff.Attack, Resources.Get<Color>("Brushes.Widgets.InsectGlaive.AttackExtract")),
        new(KinsectBuff.Speed, Resources.Get<Color>("Brushes.Widgets.InsectGlaive.SpeedExtract")),
        new(KinsectBuff.Defense, Resources.Get<Color>("Brushes.Widgets.InsectGlaive.DefenseExtract")),
        new(KinsectBuff.Heal, Resources.Get<Color>("Brushes.Widgets.InsectGlaive.HealExtract")),
        new(KinsectBuff.None, Resources.Get<Color>("Brushes.Widgets.InsectGlaive.NoExtract"))
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            KinsectBuff buff => KinsectBuffColors.First(it => it.Key == buff).Value,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}