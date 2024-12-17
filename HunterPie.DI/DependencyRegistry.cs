using System.Reflection;

namespace HunterPie.DI;

public class DependencyRegistry : IDependencyRegistry
{
    private readonly Dictionary<Type, object> _singletons = new();
    private readonly Dictionary<Type, Type> _dependencies = new();

    public T Get<T>() where T : class
    {
        return Get(typeof(T)) as T ?? throw new ArgumentException();
    }

    public object Get(Type type)
    {
        if (_singletons.ContainsKey(type))
            return _singletons[type];

        if (!_dependencies.ContainsKey(type))
            throw new ArgumentException($"Type {type.Name} has not been registered as dependency");

        return Create(_dependencies[type]);
    }

    public IDependencyRegistry WithService<TInterface, TImpl>() where TImpl : TInterface
    {
        _dependencies[typeof(TInterface)] = typeof(TImpl);

        return this;
    }

    public IDependencyRegistry WithService<TImpl>() where TImpl : class
    {
        _dependencies[typeof(TImpl)] = typeof(TImpl);

        return this;
    }

    public IDependencyRegistry WithSingle<TInterface, TImpl>() where TImpl : TInterface
    {
        _singletons[typeof(TInterface)] = Create(typeof(TImpl));

        return this;
    }

    public IDependencyRegistry WithSingle<TImpl>() where TImpl : class
    {
        _singletons[typeof(TImpl)] = Create(typeof(TImpl));

        return this;
    }

    private object Create(Type type)
    {
        ConstructorInfo? constructor = type.GetConstructors()
            .MinBy(it => it.GetParameters().Length);

        if (constructor is not { })
            throw new ArgumentException($"Type {type.Name} has not been registered as dependency");

        object[] args = constructor.GetParameters()
            .Select(param => Get(param.ParameterType))
            .ToArray();

        object instance = constructor.Invoke(args);

        return instance;
    }
}