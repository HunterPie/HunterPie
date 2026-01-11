namespace HunterPie.DI.Exceptions;

public class SingletonsCreationException(IEnumerable<Type> types, Exception[] innerExceptions) : Exception($"Failed to create instances for {string.Join(", ", types.Select(it => it.Name))}")
{

    public readonly Exception[] InnerExceptions = innerExceptions;
}