using HunterPie.DI.Modules;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.DI;

internal class DependencyProvider : IInitializer
{
    public static readonly IDependencyRegistry Registry = DependencyRegistryBuilder.Create();

    private readonly IDependencyModule[] _modules = {
        new HunterPieModule()
    };

    public Task Init()
    {
        foreach (IDependencyModule module in _modules)
            module.Register(Registry);

        return Task.CompletedTask;
    }
}