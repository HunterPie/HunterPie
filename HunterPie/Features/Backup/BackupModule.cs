using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Backup.Services;
using HunterPie.Features.Backup.Strategies;

namespace HunterPie.Features.Backup;

internal class BackupModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<MHRBackupStrategy>()
            .WithSingle<GameSaveBackupService>();
    }
}