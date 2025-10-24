using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

internal class SpecializedToolTypeToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is SpecializedToolType type
            ? type switch
            {
                SpecializedToolType.None or
                SpecializedToolType.GhillieMantle or
                SpecializedToolType.TemporalMantle or
                SpecializedToolType.RocksteadyMantle or
                SpecializedToolType.ChallengerMantle or
                SpecializedToolType.VitalityMantle or
                SpecializedToolType.FireproofMantle or
                SpecializedToolType.WaterproofMantle or
                SpecializedToolType.IceproofMantle or
                SpecializedToolType.ThunderproofMantle or
                SpecializedToolType.DragonproofMantle or
                SpecializedToolType.GliderMantle or
                SpecializedToolType.EvasionMantle or
                SpecializedToolType.ImpactMantle or
                SpecializedToolType.ApothecaryMantle or
                SpecializedToolType.ImmunityMantle or
                SpecializedToolType.BanditMantle or
                SpecializedToolType.MendingMantle or
                SpecializedToolType.CorruptedMantle => Resources.Icon("Icons.Tools.Mantle"),

                SpecializedToolType.HealthBooster or
                SpecializedToolType.AffinityBooster or
                SpecializedToolType.CleanserBooster => Resources.Icon("Icons.Tools.Booster"),

                SpecializedToolType.AssassinsHood => Resources.Icon("Icons.Tools.AssassinsMantle"),

                _ => null
            }
            : (object)null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}