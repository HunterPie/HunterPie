using System.Reflection;
using System.Runtime.Loader;

namespace HunterPie.Features.Plugins.Services;

internal class PluginLoadContext(string path) : AssemblyLoadContext(isCollectible: true)
{
    private readonly AssemblyDependencyResolver _resolver = new(path);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (assemblyName.Name == "HunterPie.Core" || assemblyName.Name == "HunterPie.UI")
            return Assembly.Load(assemblyName.Name);

        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        return assemblyPath switch
        {
            { } path => LoadFromAssemblyPath(path),
            _ => null
        };
    }
}
