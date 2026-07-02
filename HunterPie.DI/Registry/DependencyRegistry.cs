
using HunterPie.DI.Exceptions;
using HunterPie.DI.Registry.Beans;
using System.Collections.Concurrent;

namespace HunterPie.DI.Registry;

public sealed class DependencyRegistry : IScopedDependencyRegistry
{

    private readonly DependencyRegistry? _owner;
    private readonly ConcurrentDictionary<Type, List<IDependencyBean>> _beans = new();

    public DependencyRegistry() { }

    private DependencyRegistry(DependencyRegistry owner)
    {
        _owner = owner;
    }

    /// <inheritdoc />
    public T Get<T>() where T : class => (T)Get(typeof(T));

    /// <inheritdoc />
    public object Get(Type type)
    {
        List<IDependencyBean> beans = GetBeans(type);

        if (beans.Count <= 0)
            throw new DependencyNotRegisteredException(type);

        return beans
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
        _ = _beans.TryGetValue(type, out List<IDependencyBean>? beans);

        List<IDependencyBean> ownerBeans = _owner?.GetBeans(type) ?? [];

        return [.. beans ?? [], .. ownerBeans];
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

    /// <inheritdoc />
    public IScopedDependencyRegistry NewScope()
    {
        return new DependencyRegistry(this);
    }

    /// <inheritdoc />
    public IDependencyRegistry WithFactory(Type type, Activator<object> activator)
    {
        RegisterBean(
            type: type,
            bean: new FactoryDependencyBean<object>(activator)
        );

        return this;
    }


    /// <inheritdoc />
    public IDependencyRegistry WithSingle(Type type, Activator<object> activator)
    {
        RegisterBean(
            type: type,
            bean: new SingletonDependencyBean<object>(activator)
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

    public void Dispose()
    {
        _beans.Clear();
    }
}