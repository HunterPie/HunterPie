namespace HunterPie.DI;

public class DependencyRegistryBuilder
{
    public static IDependencyRegistry Create()
    {
        return new DependencyRegistry();
    }
}