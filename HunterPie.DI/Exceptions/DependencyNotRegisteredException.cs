namespace HunterPie.DI.Exceptions;

public class DependencyNotRegisteredException(Type type) : Exception($"Type {type.Name} has not been registered as dependency")
{
}