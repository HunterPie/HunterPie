using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game.Enums;
using HunterPie.DI;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters.Localizations;

#nullable enable
public class SpecializedToolToNameConverter : IValueConverter
{
    private ILocalizationRepository LocalizationRepository => DependencyContainer.Get<ILocalizationRepository>();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SpecializedToolType typ)
            return "";

        string? localizationId = typ switch
        {
            SpecializedToolType.None => null,
            SpecializedToolType.GhillieMantle => "GHILLIE_MANTLE",
            SpecializedToolType.TemporalMantle => "TEMPORAL_MANTLE",
            SpecializedToolType.HealthBooster => "HEALTH_BOOSTER",
            SpecializedToolType.RocksteadyMantle => "ROCKSTEADY_MANTLE",
            SpecializedToolType.ChallengerMantle => "CHALLENGER_MANTLE",
            SpecializedToolType.VitalityMantle => "VITALITY_MANTLE",
            SpecializedToolType.FireproofMantle => "FIREPROOF_MANTLE",
            SpecializedToolType.WaterproofMantle => "WATERPROOF_MANTLE",
            SpecializedToolType.IceproofMantle => "ICEPROOF_MANTLE",
            SpecializedToolType.ThunderproofMantle => "THUNDERPROOF_MANTLE",
            SpecializedToolType.DragonproofMantle => "DRAGONPROOF_MANTLE",
            SpecializedToolType.CleanserBooster => "CLEANSER_BOOSTER",
            SpecializedToolType.GliderMantle => "GLIDER_MANTLE",
            SpecializedToolType.EvasionMantle => "EVASION_MANTLE",
            SpecializedToolType.ImpactMantle => "IMPACT_MANTLE",
            SpecializedToolType.ApothecaryMantle => "APOTHECARY_MANTLE",
            SpecializedToolType.ImmunityMantle => "IMMUNITY_MANTLE",
            SpecializedToolType.AffinityBooster => "AFFINITY_BOOSTER",
            SpecializedToolType.BanditMantle => "BANDIT_MANTLE",
            SpecializedToolType.AssassinsHood => "ASSASSINS_HOOD",
            SpecializedToolType.MendingMantle => "MENDING_MANTLE",
            SpecializedToolType.CorruptedMantle => "CORRUPTED_MANTLE",
            _ => null
        };

        if (localizationId is null)
            return "";

        return LocalizationRepository.FindStringBy($"//Strings/SpecializedTools/Tool[@Id='{localizationId}']");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}