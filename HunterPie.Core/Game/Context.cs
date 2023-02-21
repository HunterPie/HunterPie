using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Entity.Game;
using System;

namespace HunterPie.Core.Game;

public class Context : IContext, IDisposable
{
    public IGame Game { get; protected set; }
    public IProcessManager Process { get; protected set; }

    public virtual void Dispose()
    {
        if (Game is IDisposable game)
            game.Dispose();

        GC.SuppressFinalize(this);
    }
}
