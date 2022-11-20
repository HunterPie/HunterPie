using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Data;
using HunterPie.Domain.Interfaces;
using HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo;
using System.Threading.Tasks;

namespace HunterPie.Game.Demos.Sunbreak;

internal class MHRSunbreakDemoContextInitializer : IContextInitializer
{

    public async Task InitializeAsync(IContext context)
    {
        if (context is not MHRSunbreakDemoContext)
            return;

        InitializeGameData();
    }

    private static void InitializeGameData()
    {
        MonsterData.Init(
            ClientInfo.GetPathFor("Game/Rise/Data/MonsterData.xml")
        );

        AbnormalityData.Init(
            ClientInfo.GetPathFor("Game/Rise/Data/AbnormalityData.xml")
        );
    }
}
