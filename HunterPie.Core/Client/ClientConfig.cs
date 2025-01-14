using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Observability.Logging;

namespace HunterPie.Core.Client;

#nullable enable
public static class ClientConfig
{
    private static readonly ILogger Logger = LoggerFactory.Create();
    public const string CONFIG_NAME = "config.json";

    public static V4Config Config { get; } = new();

    internal static void Initialize()
    {
        ConfigManager.Register(CONFIG_NAME, Config);

        Logger.Info("Initialized HunterPie Client configuration.");
    }
}