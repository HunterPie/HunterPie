using HunterPie.Core.Game;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Backups;
using HunterPie.Features.Statistics;
using HunterPie.Game.Rise;
using HunterPie.Game.World;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features;

internal static class ContextInitializers
{
    private static readonly IContextInitializer[] Initializers =
    {
        new MHWContextInitializer(),
        new MHRContextInitializer(),

        new GameSaveBackupService(),
        new QuestTrackerService(),
    };

    public static async Task InitializeAsync(Context context)
    {
        foreach (IContextInitializer initializer in Initializers)
            await initializer.InitializeAsync(context).ConfigureAwait(false);
    }

    public static void Dispose()
    {
        foreach (IDisposable disposable in Initializers.Where(it => it is IDisposable)
                     .Cast<IDisposable>()
                     .ToArray())
            disposable.Dispose();
    }
}