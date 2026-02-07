namespace HunterPie.DI.Registry.Beans;

internal class FactoryDependencyBean<T>(
    Activator<T> activator
) : IDependencyBean where T : notnull
{
    private readonly Activator<T> _activator = activator;

    public object Create(IDependencyRegistry registry) => _activator(registry);
}