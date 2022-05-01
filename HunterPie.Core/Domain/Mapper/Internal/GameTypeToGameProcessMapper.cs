using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.Core.Domain.Mapper.Internal
{
    internal class GameTypeToGameProcessMapper : IMapper<GameType, GameProcess>
    {
        public GameProcess Map(GameType data)
        {
            return data switch
            {
                GameType.Rise => GameProcess.MonsterHunterRise,
                GameType.World => GameProcess.MonsterHunterWorld,
                _ => GameProcess.None
            };
        }
    }
}
