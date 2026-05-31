using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;

namespace HunterPie.Integrations.Services;

public interface IGameContextFactory
{
    public Context Create(IGameProcess game);
}