using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Client
{
    public static class ClientConfigHelper
    {
        public static OverlayConfig GetOverlayConfigFrom(GameProcess game)
        {
            return game switch
            {
                GameProcess.MonsterHunterRise => ClientConfig.Config.Rise.Overlay,
                GameProcess.MonsterHunterWorld => ClientConfig.Config.World.Overlay,
                _ => throw new NotImplementedException(),
            };
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
    }
}
