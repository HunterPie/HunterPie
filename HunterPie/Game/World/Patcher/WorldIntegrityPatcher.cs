using HunterPie.Core.Observability.Logging;

namespace HunterPie.Game.World.Patcher;

internal class WorldIntegrityPatcher
{
    private static readonly ILogger _logger = LoggerFactory.Create();

    public void Patch()
    {
        _logger.Warning(
            "Make sure you have Stracker's Loader and it's dependencies installed.\n" +
            "If your game crash while using HunterPie, that means you don't have them installed."
        );
    }
}