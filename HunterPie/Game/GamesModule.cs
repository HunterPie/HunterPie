using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Game.Common;
using HunterPie.Game.Rise;
using HunterPie.Game.World;

namespace HunterPie.Game;

internal class GamesModule : IScopedModule
{
    public void Register(IScopedDependencyRegistry registry)
    {
        IContext ctx = registry.Get<IContext>();

        _ = ctx.Process.Type switch
        {
            GameProcessType.MonsterHunterWorld => registry.WithSingle<MHWContextInitializer>(),
            GameProcessType.MonsterHunterRise => registry.WithSingle<MHRContextInitializer>(),
            _ => registry.WithSingle<DisabledGameContextInitializer>()
        };
    }
}