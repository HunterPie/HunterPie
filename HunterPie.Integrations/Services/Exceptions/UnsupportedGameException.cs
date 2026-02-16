namespace HunterPie.Integrations.Services.Exceptions;

public class UnsupportedGameException(string name) : Exception($"Unsupported game {name}")
{
}