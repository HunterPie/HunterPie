using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters
{
    internal class SpecializedToolTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SpecializedToolType type)
            {
                return type switch
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
                    SpecializedToolType.BanditMantle => Resources.Icon("ICON_MANTLE"),


                    SpecializedToolType.HealthBooster or
                    SpecializedToolType.AffinityBooster or
                    SpecializedToolType.CleanserBooster => Resources.Icon("ICON_BOOSTER"),

                    SpecializedToolType.AssassinsHood => Resources.Icon("ICON_ASSASSINS_HOOD"),

                    _ => null
                };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
