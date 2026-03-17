using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Plugins.Entity;

namespace HunterPie.Playground.Plugin;

internal class ExamplePlugin : IPlugin
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public Task InitializeAsync(IContext context)
    {
        _logger.Info("This is an example plugin!");

        return Task.CompletedTask;
    }

    public void Dispose() { }

}
