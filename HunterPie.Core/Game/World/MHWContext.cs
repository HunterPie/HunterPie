using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
