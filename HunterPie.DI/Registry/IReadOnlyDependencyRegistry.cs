namespace HunterPie.DI.Registry;

public delegate void DependencyOverride(IWriteOnlyDependencyRegistry registry);

public interface IReadOnlyDependencyRegistry
{
    /// <summary>
    /// Retrieves an implementation from the dependency registry
    /// </summary>
    /// <typeparam name="T">Type of implementation</typeparam>
    /// <param name="override">Registry to override the resolution of dependencies</param>
    /// <returns>Implementation</returns>
    public T Get<T>(DependencyOverride? @override = null) where T : class;

    /// <summary>
    /// Retrieves all implementations from the dependency registry
    /// </summary>
    /// <typeparam name="T">Type of implementation</typeparam>
    /// <param name="override">Registry to override the resolution of dependencies</param>
    /// <returns>All implementations</returns>
    public T[] GetAll<T>(DependencyOverride? @override = null) where T : class;

    /// <summary>
    /// Retrieves an implementation from the dependency registry
    /// </summary>
    /// <param name="type">Type of implementation</param>
    /// <returns>Implementation</returns>
    public object Get(Type type, DependencyOverride? @override = null);

    /// <summary>
    /// Retrieves all implementations from the dependency registry
    /// </summary>
    /// <param name="type">Type of implementation</param>
    /// <returns>All implementations</returns>
    public Array GetAll(Type type, DependencyOverride? @override = null);
}
