using System;

namespace HunterPie.Core.Extensions;
public static class DateTimeExtensions
{
    public static DateTime ToLocalTime(this DateTime time)
    {
        return time - (DateTime.UtcNow - DateTime.Now);
    }
}
