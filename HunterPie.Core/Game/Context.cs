using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Entity.Game;
using System;

namespace HunterPie.Core.Game;

public abstract class Context : IContext, IDisposable
{
    public IGame Game { get; protected set; }
    public IProcessManager Process { get; protected set; }

    public abstract void Dispose();
}
