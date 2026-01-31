namespace HunterPie.DI.Registry;

public delegate T Activator<T>(IDependencyRegistry registry) where T : notnull;