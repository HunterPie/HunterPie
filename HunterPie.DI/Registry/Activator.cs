using HunterPie.DI.Exceptions;
using System.Reflection;

namespace HunterPie.DI.Registry;

public delegate T Activator<T>(IDependencyRegistry registry) where T : notnull;

public static class ReflectionActivator
{
    public static T Create<T>(IDependencyRegistry registry) where T : notnull
    {
        Type type = typeof(T);

        ConstructorInfo? constructor = type.GetConstructors()
            .MinBy(it => it.GetParameters().Length)
            ?? throw new ArgumentException($"Type {type.Name} has no accessible constructors");

        try
        {
            object[] args = constructor.GetParameters()
            .Select(it =>
            {
                return it.ParameterType.IsArray
                    ? registry.GetAll(it.ParameterType.GetElementType()!)
                    : registry.Get(it.ParameterType);
            }).ToArray();

            return (T)constructor.Invoke(args);
        }
        catch (DependencyNotRegisteredException)
        {
            throw new BeanActivationException(type);
        }

    }
}