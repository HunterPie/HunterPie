using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Game.Common;
using HunterPie.Game.Rise;
using HunterPie.Game.Rise.Patcher;
using HunterPie.Game.World;
using HunterPie.Game.World.Patcher;

namespace HunterPie.Game;

internal class GamesModule : IScopedModule
{
    public void Register(IScopedDependencyRegistry registry)
    {
        IContext ctx = registry.Get<IContext>();

        registry
            .WithSingle<IGameContextInitializer>(r => ctx.Process.Type switch
            {
                GameProcessType.MonsterHunterRise => new MHRContextInitializer(
                    context: ctx,
                    config: r.Get<ClientConfig>(),
                    patcher: r.Get<RiseIntegrityPatcher>()
                ),
                GameProcessType.MonsterHunterWorld => new MHWContextInitializer(
                    context: ctx,
                    config: r.Get<ClientConfig>(),
                    patcher: r.Get<WorldIntegrityPatcher>()
                ),
                _ => new DisabledGameContextInitializer(),
            })
            .WithSingle<MHWContextInitializer>()
            .WithSingle<MHRContextInitializer>();
    }
}