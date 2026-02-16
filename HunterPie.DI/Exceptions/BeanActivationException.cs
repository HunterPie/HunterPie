namespace HunterPie.DI.Exceptions;

public class BeanActivationException(Type type) : Exception($"Failed to create bean of type {type.Name}");