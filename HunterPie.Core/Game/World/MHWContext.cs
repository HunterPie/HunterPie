using HunterPie.Core.Domain.Process;
using System;

namespace HunterPie.Core.Game.World
{
    public class MHWContext : Context
    {
        internal MHWContext(IProcessManager process)
        {
            Game = new MHWGame(process);
            Process = process;
            // TODO: Strings
        }

    }
}
