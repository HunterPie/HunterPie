namespace HunterPie.DI.Registry;

public interface IWriteOnlyDependencyRegistry
{
    /// <summary>
    /// Registers a new dependency with type T as a factory bean, that means a new instance will be created whenever Get is called
    /// </summary>
    /// <typeparam name="T">Type to be registered</typeparam>
    /// <param name="activator">Class activator</param>
    /// <returns>The dependency registry</returns>
    public IDependencyRegistry WithFactory<T>(Activator<T>? activator = null) where T : class;

    /// <summary>
    /// Registers a new dependency where Type can only be resolved in runtime as a factory bean, that means a new instance will be created whenever Get is called
    /// </summary>
    /// <param name="type">Type to be registered</param>
    /// <param name="activator">Class activator</param>
    /// <returns>The dependency registry</returns>
    public IDependencyRegistry WithFactory(Type type, Activator<object> activator);

    /// <summary>
    /// Registers a new dependency with type T as a singleton bean, that means the same instance will be returned whenever Get is called
    /// </summary>
    /// <typeparam name="T">Type to be registered</typeparam>
    /// <param name="activator">Class activator</param>
    /// <returns>The dependency registry</returns>
    public IDependencyRegistry WithSingle<T>(Activator<T>? activator = null) where T : class;

    /// <summary>
    /// Registers a new dependency where Type can only be resolved in runtime as a factory bean, that means the same instance will be returned whenever Get is called
    /// </summary>
    /// <param name="type">Type to be registered</param>
    /// <param name="activator">Class activator</param>
    /// <returns>The dependency registry</returns>
    public IDependencyRegistry WithSingle(Type type, Activator<object> activator);

    /// <summary>
    /// Creates a new scope for the dependency registry, dependencies registered in the new scope will not be visible from the parent scope, 
    /// but dependencies registered in the parent scope will be visible from the new scope
    /// </summary>
    /// <returns>The new dependency registry scope</returns>
    public IScopedDependencyRegistry NewScope();
}