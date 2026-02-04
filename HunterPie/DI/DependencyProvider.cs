using HunterPie.Core.Observability.Logging;
using HunterPie.DI.Module;
using HunterPie.DI.Registry;
using System;
using System.Diagnostics;
using System.Linq;

namespace HunterPie.DI;

internal static class DependencyProvider
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private static readonly Lazy<IDependencyModule[]> Modules = new(() =>
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(types => typeof(IDependencyModule).IsAssignableFrom(types) && !types.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IDependencyModule>()
            .ToArray();
    });

    public static void LoadModules()
    {
        DependencyRegistry registry = new();
        var sw = new Stopwatch();
        sw.Start();

        foreach (IDependencyModule module in Modules.Value)
        {
            TimeSpan start = sw.Elapsed;
            module.Register(registry);
            Logger.Debug($"Loaded module {module.GetType().Name} in {(sw.Elapsed - start).TotalMilliseconds}ms");
        }

        DependencyContainer.SetRegistry(registry);
        Logger.Debug($"Finished loading all modules in {sw.Elapsed.TotalMilliseconds}ms");
    }
}