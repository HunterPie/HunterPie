using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;

namespace HunterPie.Integrations;

/// <summary>
/// Manager responsible to return the game context
/// </summary>
internal static class GameIntegrationService
{
    public static Context CreateNewGameContext(string processName, IProcessManager process)
    {
        // TODO: Make this a dictionary
        return processName switch
        {
            SupportedGameNames.MONSTER_HUNTER_WORLD => new MHWContext(process),
            SupportedGameNames.MONSTER_HUNTER_RISE => new MHRContext(process),
            _ => throw new Exception("Game context not implemented"),
        };
    }
}
