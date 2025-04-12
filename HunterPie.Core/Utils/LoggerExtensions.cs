using HunterPie.Core.Observability.Logging;
using System;
using System.Threading.Tasks;

namespace HunterPie.Core.Utils;

public static class LoggerExtensions
{
    public static void CatchAndLog(this ILogger logger, Action block)
    {
        try
        {
            block();
        }
        catch (Exception ex)
        {
            logger.Error(ex.ToString());
        }
    }

    public static async Task CatchAndLogAsync(this ILogger logger, Func<Task> block)
    {
        try
        {
            await block();
        }
        catch (Exception ex)
        {
            logger.Error(ex.ToString());
        }
    }
}