namespace HunterPie.DI.Module;

public interface IScopedModule
{
    public void Register(IScopedDependencyRegistry registry);
}