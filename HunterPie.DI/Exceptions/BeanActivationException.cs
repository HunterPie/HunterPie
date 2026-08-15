namespace HunterPie.DI.Exceptions;

public class BeanActivationException(Type type, Exception cause) : Exception($"Failed to create bean of type {type.Name}. Caused by {cause}");