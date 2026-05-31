using System.Threading.Tasks;

namespace HunterPie.Game.Common;

internal interface IGameContextInitializer
{
    public Task InitializeAsync();
}
