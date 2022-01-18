using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.World;
using System;

namespace HunterPie.Core.Game
{
    /// <summary>
    /// Manager responsible to return the game context
    /// </summary>
    internal static class GameManager
    {
        public static Context GetGameContext(string processName, IProcessManager process)
        {
            // TODO: Make this a dictionary
            switch (processName)
            {
                case "MonsterHunterWorld":
                    return new MHWContext(process);
                case "MonsterHunterRise":
                    return new MHRContext(process);
                default:
                    throw new Exception("Game context not implemented");
            }
        }
    }
}
