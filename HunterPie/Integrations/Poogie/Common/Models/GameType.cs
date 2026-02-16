using System;
using GameTypeEntity = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.Integrations.Poogie.Common.Models;

internal enum GameType
{
    MHR,
    MHW,
    MHWilds
}

internal static class GameTypeExtensions
{
    extension(GameType type)
    {
        public GameTypeEntity ToEntity()
        {
            return type switch
            {
                GameType.MHR => GameTypeEntity.Rise,
                GameType.MHW => GameTypeEntity.World,
                GameType.MHWilds => GameTypeEntity.Wilds,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }

    extension(GameTypeEntity type)
    {
        public GameType ToApiModel()
        {
            return type switch
            {
                GameTypeEntity.Rise => GameType.MHR,
                GameTypeEntity.World => GameType.MHW,
                GameTypeEntity.Wilds => GameType.MHWilds,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}