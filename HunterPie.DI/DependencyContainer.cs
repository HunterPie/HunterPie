using HunterPie.DI.Registry;

namespace HunterPie.DI;

public static class DependencyContainer
{
    private static readonly Lock Lock = new();

    private static IDependencyRegistry? _registry;

    public static void SetRegistry(IDependencyRegistry registry)
    {
        lock (Lock)
            _registry = registry;
    }

    public static T Get<T>(DependencyOverride? @override = null) where T : class => (T)Get(typeof(T), @override);

    public static object Get(Type type, DependencyOverride? @override = null)
    {
        if (_registry is { })
            return _registry.Get(type, @override);

        lock (Lock)
            return _registry?.Get(type, @override) ?? throw new NullReferenceException($"{nameof(DependencyContainer)} has not been initialized yet");
    }
}