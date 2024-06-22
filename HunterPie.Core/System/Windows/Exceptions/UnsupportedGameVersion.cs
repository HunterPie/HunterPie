using System;

namespace HunterPie.Core.System.Windows.Exceptions;

internal class UnsupportedGameVersion : Exception
{
    public UnsupportedGameVersion()
        : base("The game version is not supported")
    {

    }
}
