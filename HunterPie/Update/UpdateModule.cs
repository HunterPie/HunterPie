using HunterPie.DI;
using HunterPie.DI.Modules;
using HunterPie.Update.Gateway;
using HunterPie.Update.Service;
using HunterPie.Update.Usecase;

namespace HunterPie.Update;

internal class UpdateModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<IUpdateCleanUpUseCase, UpdateCleanUpService>()
            .WithService<IUpdateUseCase, UpdateService>()
            .WithService<LocalizationUpdateService>()
            .WithService<ChecksumService>()
            .WithService<UpdateGateway>();
    }
}