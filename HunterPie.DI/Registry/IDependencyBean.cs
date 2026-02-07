namespace HunterPie.DI.Registry;

internal interface IDependencyBean
{
    public object Create(IDependencyRegistry registry);
}