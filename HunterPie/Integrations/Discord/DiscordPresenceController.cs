using HunterPie.Core.Game;
using HunterPie.Core.Game.Demos.Sunbreak;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.World;
using System;

namespace HunterPie.Integrations.Discord
{
    internal static class DiscordPresenceController
    {

        public static RichPresence GetPresenceBy(Context context)
        {
            return context switch
            {
                MHWContext ctx => new WorldRichPresence(ctx),
                MHRContext ctx => new RiseRichPresence(ctx),
                MHRSunbreakDemoContext ctx => new RiseSunbreakDemoRichPresence(ctx),
                _ => throw new NotImplementedException("unreachable")
            };
        }

    }
}
