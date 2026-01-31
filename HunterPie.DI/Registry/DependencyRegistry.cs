
using HunterPie.DI.Exceptions;
using HunterPie.DI.Registry.Beans;
using System.Collections.Concurrent;

namespace HunterPie.DI.Registry;

public class DependencyRegistry : IDependencyRegistry
{
    private readonly ConcurrentDictionary<Type, List<IDependencyBean>> _beans = new();

    /// <inheritdoc />
    public T Get<T>() where T : class => (T)Get(typeof(T));

    /// <inheritdoc />
    public object Get(Type type)
    {
        return GetBeans(type)
            .First()
            .Create(this);
    }

    /// <inheritdoc />
    public T[] GetAll<T>() where T : class
    {
        return GetBeans(typeof(T))
            .Select(it => it.Create(this))
            .Cast<T>()
            .ToArray();
    }

    private List<IDependencyBean> GetBeans(Type type)
    {
        if (!_beans.TryGetValue(type, out List<IDependencyBean>? beans))
            throw new DependencyNotRegisteredException(type);

        return beans;
    }

    /// <inheritdoc />
    public IDependencyRegistry WithFactory<T>(Activator<T> activator) where T : class
    {
        Type type = typeof(T);

        _beans.AddOrUpdate(
            key: type,
            addValueFactory: (_) => new List<IDependencyBean> { new FactoryDependencyBean<T>(activator) },
            updateValueFactory: (_, dependencies) =>
            {
                dependencies.Add(new FactoryDependencyBean<T>(activator));
                return dependencies;
            }
        );

        return this;
    }

    /// <inheritdoc />
    public IDependencyRegistry WithSingle<T>(Activator<T> activator) where T : class
    {
        Type type = typeof(T);

        _beans.AddOrUpdate(
            key: type,
            addValueFactory: (_) => new List<IDependencyBean> { new SingletonDependencyBean<T>(activator) },
            updateValueFactory: (_, dependencies) =>
            {
                dependencies.Add(new SingletonDependencyBean<T>(activator));
                return dependencies;
            }
        );

        return this;
    }
}
