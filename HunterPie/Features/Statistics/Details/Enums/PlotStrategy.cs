using HunterPie.Core.Client.Localization;

namespace HunterPie.Features.Statistics.Details.Enums;

public enum PlotStrategy
{
    [Localization("//Strings/Client/Enums/Enum[@Id='PLOT_SLIDING_AVG']")]
    AverageMoving,

    [Localization("//Strings/Client/Enums/Enum[@Id='PLOT_OVERALL_AVG']")]
    AverageOverall
}