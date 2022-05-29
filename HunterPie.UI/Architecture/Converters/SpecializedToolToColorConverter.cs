using HunterPie.Core.Game.World.Entities;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters
{
    public class SpecializedToolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MHWSpecializedTool tool)
            {
                return tool switch
                {
                    MHWSpecializedTool.GhillieMantle => Resources.Get<Brush>("COLOR_GHILLIE_MANTLE"),
                    MHWSpecializedTool.TemporalMantle => Resources.Get<Brush>("COLOR_TEMPORAL_MANTLE"),
                    MHWSpecializedTool.HealthBooster => Resources.Get<Brush>("COLOR_HEALTH_BOOSTER"),
                    MHWSpecializedTool.RocksteadyMantle => Resources.Get<Brush>("COLOR_ROCKSTEADY_MANTLE"),
                    MHWSpecializedTool.ChallengerMantle => Resources.Get<Brush>("COLOR_CHALLENGER_MANTLE"),
                    MHWSpecializedTool.VitalityMantle => Resources.Get<Brush>("COLOR_VITALITY_MANTLE"),
                    MHWSpecializedTool.FireproofMantle => Resources.Get<Brush>("COLOR_FIREPROOF_MANTLE"),
                    MHWSpecializedTool.WaterproofMantle => Resources.Get<Brush>("COLOR_WATERPROOF_MANTLE"),
                    MHWSpecializedTool.IceproofMantle => Resources.Get<Brush>("COLOR_ICEPROOF_MANTLE"),
                    MHWSpecializedTool.ThunderproofMantle => Resources.Get<Brush>("COLOR_THUNDERPROOF_MANTLE"),
                    MHWSpecializedTool.DragonproofMantle => Resources.Get<Brush>("COLOR_DRAGONPROOF_MANTLE"),
                    MHWSpecializedTool.CleanserBooster => Resources.Get<Brush>("COLOR_CLEANSER_BOOSTER"),
                    MHWSpecializedTool.GliderMantle => Resources.Get<Brush>("COLOR_GLINDER_MANTLE"),
                    MHWSpecializedTool.EvasionMantle => Resources.Get<Brush>("COLOR_EVASION_MANTLE"),
                    MHWSpecializedTool.ImpactMantle => Resources.Get<Brush>("COLOR_IMPACT_MANTLE"),
                    MHWSpecializedTool.ApothecaryMantle => Resources.Get<Brush>("COLOR_APOTHECARY_MANTLE"),
                    MHWSpecializedTool.ImmunityMantle => Resources.Get<Brush>("COLOR_IMMUNITY_MANTLE"),
                    MHWSpecializedTool.AffinityMantle => Resources.Get<Brush>("COLOR_AFFINITY_MANTLE"),
                    MHWSpecializedTool.BanditMantle => Resources.Get<Brush>("COLOR_BANDIT_MANTLE"),
                    MHWSpecializedTool.AssassinsHood => Resources.Get<Brush>("COLOR_ASSASSINS_HOOD"),
                    _ => throw new NotImplementedException(),
                };
            }
            throw new ArgumentException($"expected value to be of type {nameof(MHWSpecializedTool)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
