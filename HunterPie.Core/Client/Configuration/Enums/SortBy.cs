using System.ComponentModel;

namespace HunterPie.Core.Client.Configuration.Enums
{
    public enum SortBy
    {
        [Description("SORT_BY_LOWEST_TIMER_STRING")]
        Lowest,
        [Description("SORT_BY_HIGHEST_TIMER_STRING")]
        Highest,
        [Description("SORT_BY_DISABLED_STRING")]
        Off
    }
}
