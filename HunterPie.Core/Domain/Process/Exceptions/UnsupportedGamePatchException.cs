using System;

namespace HunterPie.Core.Domain.Process.Exceptions;

public class UnsupportedGamePatchException : Exception
{
    public string Game { get; }
    public string Version { get; }

    public UnsupportedGamePatchException(string game, string version) : base()
    {
        Game = game;
        Version = version;
    }
}