using HunterPie.DI.Registry;

namespace HunterPie.DI;

public interface IDependencyRegistry : IReadOnlyDependencyRegistry, IWriteOnlyDependencyRegistry;