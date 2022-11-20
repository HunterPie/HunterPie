using HunterPie.Core.Game;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Backups;
using HunterPie.Game.Demos.Sunbreak;
using HunterPie.Game.Rise;
using HunterPie.Game.World;
using System.Threading.Tasks;

namespace HunterPie.Features;

internal static class ContextInitializers
{
    private static readonly IContextInitializer[] Initializers =
    {
        new MHWContextInitializer(),
        new MHRContextInitializer(),
        new MHRSunbreakDemoContextInitializer(),

        new GameSaveBackupService(),
    };

    public static async Task InitializeAsync(Context context)
    {
        foreach (IContextInitializer initializer in Initializers)
            await initializer.InitializeAsync(context).ConfigureAwait(false);
    }
}
