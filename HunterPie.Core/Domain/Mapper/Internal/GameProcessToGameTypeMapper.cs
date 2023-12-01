using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;
using System;

namespace HunterPie.Core.Domain.Mapper.Internal;

#nullable enable
internal class GameProcessToGameTypeMapper : IMapper<GameProcess, GameType?>
{
    public GameType? Map(GameProcess data)
    {
        return data switch
        {
            GameProcess.None => null,
            GameProcess.MonsterHunterRise => GameType.Rise,
            GameProcess.MonsterHunterWorld => GameType.World,
            GameProcess.MonsterHunterRiseSunbreakDemo => GameType.Rise,
            GameProcess.All => null,
            _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
        };
    }
}