using HunterPie.Core.Client.Configuration.Enums;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Assets;

public interface IMonsterIconResolver
{
    public Task<string?> Get(
        GameType game,
        int monsterId,
        bool isQurio = false
    );
}
