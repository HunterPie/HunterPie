using HunterPie.Core.Observability.Logging;
using HunterPie.DI.Module;
using HunterPie.DI.Registry;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace HunterPie.DI;

internal static class DependencyProvider
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private static readonly Lazy<ImmutableArray<IDependencyModule>> Modules = new(ReflectScan<IDependencyModule>);

    private static readonly Lazy<ImmutableArray<IScopedModule>> ScopedModules = new(ReflectScan<IScopedModule>);

    internal static void LoadModules()
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

    internal static void LoadScopedModules(IScopedDependencyRegistry registry)
    {
        var sw = new Stopwatch();
        sw.Start();

        foreach (IScopedModule module in ScopedModules.Value)
        {
            TimeSpan start = sw.Elapsed;
            module.Register(registry);
            Logger.Debug($"Loaded module {module.GetType().Name} in {(sw.Elapsed - start).TotalMilliseconds}ms");
        }
    }

    private static ImmutableArray<T> ReflectScan<T>()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(types => typeof(T).IsAssignableFrom(types) && !types.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<T>()
            .ToImmutableArray();
    }
}