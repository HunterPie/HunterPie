using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game.Entity.Game;
using System;

namespace HunterPie.Core.Game;

public class Context(
    IGame game,
    IGameProcess process
) : IContext, IDisposable
{
    public IGame Game { get; } = game;

    public IGameProcess Process { get; } = process;

    public virtual void Dispose()
    {
        if (Game is IDisposable game)
            game.Dispose();

        GC.SuppressFinalize(this);
    }
}