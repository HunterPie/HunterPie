using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WPFBrushes = System.Windows.Media.Brushes;

namespace HunterPie.UI.Architecture.Converters.Brushes;

[ValueConversion(typeof(GameType), typeof(Brush))]
public class GameTypeToBrushConverter : IValueConverter
{
    private static readonly Lazy<Brush> WildsBrush = new(() => Resources.Get<Brush>("Brushes.Game.MonsterHunterWilds"));
    private static readonly Lazy<Brush> RiseBrush = new(() => Resources.Get<Brush>("Brushes.Game.MonsterHunterRise"));
    private static readonly Lazy<Brush> WorldBrush = new(() => Resources.Get<Brush>("Brushes.Game.MonsterHunterWorld"));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is GameType gameType)
        {
            return gameType switch
            {
                GameType.Rise => RiseBrush.Value,
                GameType.World => WorldBrush.Value,
                GameType.Wilds => WildsBrush.Value,
                _ => WPFBrushes.White,
            };
        }

        return WPFBrushes.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}