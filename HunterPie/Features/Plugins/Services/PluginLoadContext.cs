using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace HunterPie.Features.Plugins.Services;

internal class PluginLoadContext(string path) : AssemblyLoadContext(isCollectible: true)
{
    private static readonly string[] CoreLibraries = [
        "HunterPie.UI",
        "HunterPie.Core",
        "HunterPie.DI",
        "HunterPie.Integrations",
        "HunterPie.Platforms"
    ];

    private readonly AssemblyDependencyResolver _resolver = new(path);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? name = assemblyName.Name;

        if (name is { } && CoreLibraries.Contains(name))
            return Assembly.Load(name);

        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        return assemblyPath switch
        {
            { } path => LoadFromAssemblyPath(path),
            _ => null
        };
    }
}
