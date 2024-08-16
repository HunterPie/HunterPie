using System;

namespace HunterPie.Core.System.Windows.Exceptions;

public class UnsupportedGameVersion : Exception
{
    public UnsupportedGameVersion()
        : base("The game version is not supported")
    {

    }
}
