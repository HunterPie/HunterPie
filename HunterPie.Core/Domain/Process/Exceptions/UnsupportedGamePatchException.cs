using System;

namespace HunterPie.Core.Domain.Process.Exceptions;

public class UnsupportedGamePatchException(string game, string version) : Exception()
{
    public string Game { get; } = game;
    public string Version { get; } = version;
}