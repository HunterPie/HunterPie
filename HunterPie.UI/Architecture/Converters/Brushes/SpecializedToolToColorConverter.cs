using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using PrimitiveColor = System.Windows.Media.Colors;

namespace HunterPie.UI.Architecture.Converters.Brushes;

public class SpecializedToolToColorConverter : IValueConverter
{
    private static readonly Brush DefaultColor = new SolidColorBrush(PrimitiveColor.White);

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is SpecializedToolType tool
            ? tool switch
            {
                SpecializedToolType.None => DefaultColor,
                SpecializedToolType.GhillieMantle => Resources.Get<Brush>("Brushes.Tools.GhillieMantle"),
                SpecializedToolType.TemporalMantle => Resources.Get<Brush>("Brushes.Tools.TemporalMantle"),
                SpecializedToolType.HealthBooster => Resources.Get<Brush>("Brushes.Tools.HealthBooster"),
                SpecializedToolType.RocksteadyMantle => Resources.Get<Brush>("Brushes.Tools.RocksteadyMantle"),
                SpecializedToolType.ChallengerMantle => Resources.Get<Brush>("Brushes.Tools.ChallengerMantle"),
                SpecializedToolType.VitalityMantle => Resources.Get<Brush>("Brushes.Tools.VitalityMantle"),
                SpecializedToolType.FireproofMantle => Resources.Get<Brush>("Brushes.Tools.FireproofMantle"),
                SpecializedToolType.WaterproofMantle => Resources.Get<Brush>("Brushes.Tools.WaterproofMantle"),
                SpecializedToolType.IceproofMantle => Resources.Get<Brush>("Brushes.Tools.IceproofMantle"),
                SpecializedToolType.ThunderproofMantle => Resources.Get<Brush>("Brushes.Tools.ThunderproofMantle"),
                SpecializedToolType.DragonproofMantle => Resources.Get<Brush>("Brushes.Tools.DragonproofMantle"),
                SpecializedToolType.CleanserBooster => Resources.Get<Brush>("Brushes.Tools.CleanserBooster"),
                SpecializedToolType.GliderMantle => Resources.Get<Brush>("Brushes.Tools.GliderMantle"),
                SpecializedToolType.EvasionMantle => Resources.Get<Brush>("Brushes.Tools.EvasionMantle"),
                SpecializedToolType.ImpactMantle => Resources.Get<Brush>("Brushes.Tools.ImpactMantle"),
                SpecializedToolType.ApothecaryMantle => Resources.Get<Brush>("Brushes.Tools.ApothecaryMantle"),
                SpecializedToolType.ImmunityMantle => Resources.Get<Brush>("Brushes.Tools.ImmunityMantle"),
                SpecializedToolType.AffinityBooster => Resources.Get<Brush>("Brushes.Tools.AffinityBooster"),
                SpecializedToolType.BanditMantle => Resources.Get<Brush>("Brushes.Tools.BanditMantle"),
                SpecializedToolType.AssassinsHood => Resources.Get<Brush>("Brushes.Tools.AssassinsHood"),
                SpecializedToolType.MendingMantle => Resources.Get<Brush>("Brushes.Tools.MendingMantle"),
                SpecializedToolType.CorruptedMantle => Resources.Get<Brush>("Brushes.Tools.CorruptedMantle"),
                _ => throw new NotImplementedException(),
            }
            : throw new ArgumentException($"expected value to be of type {nameof(SpecializedToolType)}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}