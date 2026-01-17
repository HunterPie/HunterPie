namespace HunterPie.DI.Exceptions;

public class DependencyArgumentException(Type type, Exception innerException) : Exception(
    $"Failed to create instance of {type.Name} due to argument exception", innerException)
{
}