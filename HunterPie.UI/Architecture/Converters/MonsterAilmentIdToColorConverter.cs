using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters;

#nullable enable
public class MonsterAilmentIdToColorConverter : IValueConverter
{

    private static Brush Default => Resources.Get<Brush>("Brushes.Ailments.Unknown");

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string? brushId = value is string str
            ? str switch
            {
                "STATUS_ENRAGE" => "Enrage",
                "AILMENT_EXHAUST" => "Exhaust",
                "AILMENT_POISON" => "Poison",
                "AILMENT_PARALYSIS" => "Paralysis",
                "AILMENT_SLEEP" => "Sleep",
                "AILMENT_BLAST" => "Blast",
                "AILMENT_MOUNT" => "Mount",
                "AILMENT_STUN" => "Stun",
                "AILMENT_TRANQUILIZE" => "Tranquilize",
                "AILMENT_FLASH" => "Flash",
                "AILMENT_DUNG" => "Dung",
                "AILMENT_WATER" => "Water",
                "AILMENT_FIRE" => "Fire",
                "AILMENT_ICE" => "Ice",
                "AILMENT_THUNDER" => "Thunder",
                "AILMENT_SMOKING" => "Smoking",
                "AILMENT_KNOCKDOWN" => "Knockdown",
                "AILMENT_CLAW" => "Claw",
                "AILMENT_ELDERSEAL" => "Elderseal",
                _ => null
            }
            : null;

        if (brushId is null)
            return Default;

        return Resources.TryGet<Brush>($"Brushes.Ailments.{brushId}") ?? Default;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}