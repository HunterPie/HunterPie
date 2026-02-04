using HunterPie.DI.Registry;

namespace HunterPie.DI;

public interface IDependencyRegistry
{
    /// <summary>
    /// Retrieves an implementation from the dependency registry
    /// </summary>
    /// <typeparam name="T">Type of implementation</typeparam>
    /// <returns>Implementation</returns>
    public T Get<T>() where T : class;

    /// <summary>
    /// Retrieves all implementations from the dependency registry
    /// </summary>
    /// <typeparam name="T">Type of implementation</typeparam>
    /// <returns>All implementations</returns>
    public T[] GetAll<T>() where T : class;

    /// <summary>
    /// Retrieves an implementation from the dependency registry
    /// </summary>
    /// <param name="type">Type of implementation</param>
    /// <returns>Implementation</returns>
    public object Get(Type type);

    /// <summary>
    /// Retrieves all implementations from the dependency registry
    /// </summary>
    /// <param name="type">Type of implementation</param>
    /// <returns>All implementations</returns>
    public Array GetAll(Type type);

    /// <summary>
    /// Registers a new dependency with type T as a factory bean, that means a new instance will be created whenever Get is called
    /// </summary>
    /// <typeparam name="T">Type to be registered</typeparam>
    /// <param name="activator">Class activator</param>
    /// <returns>The dependency registry</returns>
    public IDependencyRegistry WithFactory<T>(Activator<T>? activator = null) where T : class;


    /// <summary>
    /// Registers a new dependency with type T as a singleton bean, that means the same instance will be returned whenever Get is called
    /// </summary>
    /// <typeparam name="T">Type to be registered</typeparam>
    /// <param name="activator">Class activator</param>
    /// <returns>The dependency registry</returns>
    public IDependencyRegistry WithSingle<T>(Activator<T>? activator = null) where T : class;


}