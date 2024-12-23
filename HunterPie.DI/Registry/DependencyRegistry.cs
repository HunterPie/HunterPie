using HunterPie.DI.Exceptions;
using System.Reflection;

namespace HunterPie.DI.Registry;

public class DependencyRegistry : IDependencyRegistry
{
    private readonly Queue<Dependency> _singletonQueue = new();
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
            throw new DependencyNotRegisteredException(type);

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
        _singletonQueue.Enqueue(
            item: new Dependency(
                ConcreteType: typeof(TImpl),
                AbstractType: typeof(TInterface)
            )
        );

        return this;
    }

    public IDependencyRegistry WithSingle<TImpl>() where TImpl : class
    {
        _singletonQueue.Enqueue(
            item: new Dependency(
                ConcreteType: typeof(TImpl),
                AbstractType: typeof(TImpl)
            )
        );
        return this;
    }

    public IDependencyRegistry WithSingle<T>(T impl) where T : class
    {
        _singletons[typeof(T)] = impl;

        return this;
    }

    public IDependencyRegistry Build()
    {
        // TODO: Change this to build a dependency tree instead of this disgusting brute force approach
        // I actually despise this approach
        while (true)
        {
            int count = _singletonQueue.Count;

            if (count <= 0)
                return this;

            var failedDependencies = new Queue<Dependency>(count);
            var exceptions = new List<Exception>();
            while (_singletonQueue.TryDequeue(out Dependency? dependency))
                try
                {
                    Type key = dependency.AbstractType;
                    object singletonInstance = Create(dependency.ConcreteType);
                    _singletons[key] = singletonInstance;
                }
                catch (DependencyArgumentException err)
                {
                    failedDependencies.Enqueue(dependency);
                    exceptions.Add(err);
                }
                catch (Exception err)
                {
                    exceptions.Add(err);
                }

            if (failedDependencies.Count >= count)
                throw new SingletonsCreationException(failedDependencies.Select(it => it.ConcreteType), exceptions.ToArray());

            while (failedDependencies.TryDequeue(out Dependency? dependency))
                _singletonQueue.Enqueue(dependency);
        }
    }

    private object Create(Type type)
    {
        ConstructorInfo? constructor = type.GetConstructors()
            .MinBy(it => it.GetParameters().Length);

        if (constructor is not { })
            throw new ArgumentException($"Type {type.Name} contains no accessible constructors");

        object[] args = constructor.GetParameters()
            .Select(param =>
            {
                try
                {
                    return Get(param.ParameterType);
                }
                catch (DependencyNotRegisteredException err)
                {
                    throw new DependencyArgumentException(type, err);
                }
            })
            .ToArray();

        object instance = constructor.Invoke(args);

        return instance;
    }
}