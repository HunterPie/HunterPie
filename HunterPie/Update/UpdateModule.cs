using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Update.Gateway;
using HunterPie.Update.Service;

namespace HunterPie.Update;

internal class UpdateModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithFactory<UpdateCleanUpService>()
            .WithFactory<UpdateService>()
            .WithFactory<LocalizationUpdateService>()
            .WithFactory<ChecksumService>()
            .WithFactory<UpdateGateway>();
    }
}