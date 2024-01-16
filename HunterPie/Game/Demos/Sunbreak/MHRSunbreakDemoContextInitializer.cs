using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Data;
using HunterPie.Domain.Interfaces;
using HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo;
using System.Threading.Tasks;

namespace HunterPie.Game.Demos.Sunbreak;

internal class MHRSunbreakDemoContextInitializer : IContextInitializer
{

    public Task InitializeAsync(IContext context)
    {
        if (context is not MHRSunbreakDemoContext)
            return Task.CompletedTask;

        InitializeGameData();

        return Task.CompletedTask;
    }

    private static void InitializeGameData()
    {
        MonsterData.Init(
            ClientInfo.GetPathFor("Game/Rise/Data/MonsterData.xml")
        );
    }
}
