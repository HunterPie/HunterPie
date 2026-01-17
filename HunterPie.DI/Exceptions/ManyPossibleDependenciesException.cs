namespace HunterPie.DI.Exceptions;

public class ManyPossibleDependenciesException(Type type, Type[] possibilities) : Exception($"Too many possible options to satisfy dependency '{type.Name}'")
{
    public Type Type { get; } = type;
    public Type[] Possibilities { get; } = possibilities;
}