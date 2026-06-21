using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Plugins.Entity;
using HunterPie.Playground.Plugin.Configuration;

namespace HunterPie.Playground.Plugin;

internal class ExamplePlugin(
    IContext context,
    ExamplePluginConfigurationV1 config
) : IPlugin
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public Task InitializeAsync()
    {
        if (!config.IsTestLoggingEnabled)
            return Task.CompletedTask;

        _logger.Info($"This is an example plugin! The current game running is {context.Process.Name}");

        return Task.CompletedTask;
    }

    public void Dispose() { }

}