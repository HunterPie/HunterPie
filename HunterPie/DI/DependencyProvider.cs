using HunterPie.DI.Modules;
using HunterPie.DI.Registry;

namespace HunterPie.DI;

internal static class DependencyProvider
{
    private static readonly IDependencyModule[] Modules = {
        new InitializersModule(),
        new HunterPieModule()
    };

    public static void LoadModules()
    {
        DependencyRegistry registry = DependencyRegistryBuilder.Create();

        foreach (IDependencyModule module in Modules)
            module.Register(registry);

        DependencyContainer.SetRegistry(registry.Build());
    }
}