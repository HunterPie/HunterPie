
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
        return GetAll(typeof(T))
            .Cast<T>()
            .ToArray();
    }

    /// <inheritdoc />
    public Array GetAll(Type type)
    {
        object[] beans = GetBeans(type)
            .Select(it => it.Create(this))
            .ToArray();

        var array = Array.CreateInstance(type, beans.Length);
        Array.Copy(beans, array, array.Length);

        return array;
    }

    private List<IDependencyBean> GetBeans(Type type)
    {
        if (!_beans.TryGetValue(type, out List<IDependencyBean>? beans))
            throw new DependencyNotRegisteredException(type);

        return beans;
    }

    /// <inheritdoc />
    public IDependencyRegistry WithFactory<T>(Activator<T>? activator = null) where T : class
    {
        RegisterBean(
            type: typeof(T),
            bean: new FactoryDependencyBean<T>(activator ?? ReflectionActivator.Create<T>)
        );

        return this;
    }

    /// <inheritdoc />
    public IDependencyRegistry WithSingle<T>(Activator<T>? activator = null) where T : class
    {
        RegisterBean(
            type: typeof(T),
            bean: new SingletonDependencyBean<T>(activator ?? ReflectionActivator.Create<T>)
        );

        return this;
    }

    private void RegisterBean(Type type, IDependencyBean bean)
    {
        Type[] innerTypes = [.. type.GetInterfaces(), type];

        foreach (Type innerType in innerTypes)
            _beans.AddOrUpdate(
                key: innerType,
                addValueFactory: (_) => new List<IDependencyBean>() { bean },
                updateValueFactory: (_, dependencies) =>
                {
                    dependencies.Add(bean);
                    return dependencies;
                }
            );
    }
}