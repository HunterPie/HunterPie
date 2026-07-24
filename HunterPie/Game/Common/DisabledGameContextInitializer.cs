using System.Threading.Tasks;

namespace HunterPie.Game.Common;

internal class DisabledGameContextInitializer : IGameContextInitializer
{
    public Task InitializeAsync() => Task.CompletedTask;
}