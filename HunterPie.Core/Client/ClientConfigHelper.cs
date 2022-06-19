using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Client
{
    public static class ClientConfigHelper
    {
        public delegate T OverlayConfigDeferDelegate<T>(OverlayConfig config);

        public static OverlayConfig GetOverlayConfigFrom(GameProcess game)
        {
            return game switch
            {
                GameProcess.MonsterHunterRiseSunbreakDemo or
                GameProcess.MonsterHunterRise => ClientConfig.Config.Rise.Overlay,

                GameProcess.MonsterHunterWorld => ClientConfig.Config.World.Overlay,
                _ => throw new NotImplementedException(),
            };
        }

        public static T DeferOverlayConfig<T>(GameProcess game, OverlayConfigDeferDelegate<T> deferDelegate)
        {
            OverlayConfig config = GetOverlayConfigFrom(game);

            return deferDelegate(config);
        }

        public static DiscordRichPresence GetDiscordRichPresenceConfigFrom(GameProcess game)
        {
            return game switch
            {
                GameProcess.MonsterHunterRise => ClientConfig.Config.Rise.RichPresence,
                GameProcess.MonsterHunterWorld => ClientConfig.Config.World.RichPresence,
                _ => throw new NotImplementedException(),
            };
        }

        public static GameConfig GetGameConfigBy(GameProcess game)
        {
            return game switch
            {
                GameProcess.MonsterHunterRiseSunbreakDemo or
                GameProcess.MonsterHunterRise => ClientConfig.Config.Rise,

                GameProcess.MonsterHunterWorld => ClientConfig.Config.World,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
