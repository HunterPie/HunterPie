using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.Core.Client;

public static class RegistryConfig
{
    private static ILocalRegistry _localRegistry;

    public static void Initialize(ILocalRegistry registry) => _localRegistry = registry;

    public static void Set<T>(string name, T value) => _localRegistry.Set(name, value);
    public static bool Exists(string name) => _localRegistry.Exists(name);
    public static object Get(string name) => _localRegistry.Get(name);
    public static T Get<T>(string name) => _localRegistry.Get<T>(name);
    public static void Delete(string name) => _localRegistry.Delete(name);

}