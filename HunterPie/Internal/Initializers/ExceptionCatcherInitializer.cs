using HunterPie.Core.Analytics;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Utils;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Analytics.Entity;
using System;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class ExceptionCatcherInitializer(
    AppDomain app,
    IAnalyticsService analyticsService
) : IInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public Task Init()
    {
        app.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is not Exception exception)
                return;

            AsyncHelper.RunSync(async () =>
            {
                await analyticsService.SendAsync(
                    analyticsEvent: AnalyticsEvent.FromException(exception, isUiError: false)
                );
            });


            _logger.Error(exception.ToString());
        };

        return Task.CompletedTask;
    }
}