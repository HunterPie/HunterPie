using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Client;

public static class ClientConfigHelper
{
    public delegate T OverlayConfigDeferDelegate<T>(OverlayConfig config);

    public static OverlayConfig GetOverlayConfigFrom(GameProcessType game)
    {
        return game switch
        {
            GameProcessType.MonsterHunterRise => ClientConfig.Config.Rise.Overlay,

            GameProcessType.MonsterHunterWorld => ClientConfig.Config.World.Overlay,

            GameProcessType.MonsterHunterWilds => ClientConfig.Config.Wilds.Overlay,

            _ => throw new NotImplementedException(),
        };
    }

    public static T DeferOverlayConfig<T>(GameProcessType game, OverlayConfigDeferDelegate<T> deferDelegate)
    {
        OverlayConfig config = GetOverlayConfigFrom(game);

        return deferDelegate(config);
    }

    public static GameConfig GetGameConfigBy(GameProcessType game)
    {
        return game switch
        {
            GameProcessType.MonsterHunterRise => ClientConfig.Config.Rise,

            GameProcessType.MonsterHunterWorld => ClientConfig.Config.World,

            GameProcessType.MonsterHunterWilds => ClientConfig.Config.Wilds,

            _ => throw new NotImplementedException(),
        };
    }
}