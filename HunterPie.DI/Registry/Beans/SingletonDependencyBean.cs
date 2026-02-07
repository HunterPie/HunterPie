namespace HunterPie.DI.Registry.Beans;

internal class SingletonDependencyBean<T>(
    Activator<T> activator
) : IDependencyBean where T : notnull
{
    private T? _value;
    private readonly Lock _lock = new();
    private readonly Activator<T> _activator = activator;

    public object Create(IDependencyRegistry registry)
    {
        lock (_lock)
        {
            return _value ??= _activator(registry);
        }
    }
}