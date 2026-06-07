using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Discord.Service;
using HunterPie.Integrations.Discord.Strategies;

namespace HunterPie.Integrations.Discord;

internal class DiscordModule : IScopedModule
{
    public void Register(IScopedDependencyRegistry registry)
    {
        IContext ctx = registry.Get<IContext>();

        _ = ctx.Process.Type switch
        {
            GameProcessType.MonsterHunterRise => registry.WithSingle<MHRDiscordPresenceStrategy>(),
            GameProcessType.MonsterHunterWorld => registry.WithSingle<MHWDiscordPresenceStrategy>(),
            GameProcessType.MonsterHunterWilds => registry.WithSingle<MHWildsDiscordPresenceStrategy>(),
            _ => registry.WithSingle<DisabledDiscordPresenceStrategy>()
        };

        registry
            .WithSingle<DiscordPresenceService>();
    }
}