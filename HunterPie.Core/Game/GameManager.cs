using HunterPie.Core.Client;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Demos.Sunbreak;
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
                case "MHRiseSunbreakDemo":
                    return new MHRSunbreakDemoContext(process);
                default:
                    throw new Exception("Game context not implemented");
            }
        }

        public static bool InitializeGameData(string processName)
        {
            // TODO: Refactor this
            switch (processName)
            {
                case "MonsterHunterWorld":
                    MonsterData.Init(ClientInfo.GetPathFor("Game/World/Data/MonsterData.xml"));
                    AbnormalityData.Init(ClientInfo.GetPathFor("Game/World/Data/AbnormalityData.xml"));
                    return true;
                case "MonsterHunterRise":
                case "MHRiseSunbreakDemo":
                    MonsterData.Init(ClientInfo.GetPathFor("Game/Rise/Data/MonsterData.xml"));
                    AbnormalityData.Init(ClientInfo.GetPathFor("Game/Rise/Data/AbnormalityData.xml"));
                    return true;
                default:
                    throw new Exception("Game context not implemented");
            }
        }
    }
}
