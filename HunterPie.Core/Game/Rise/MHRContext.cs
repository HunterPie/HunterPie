using HunterPie.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Rise
{
    public class MHRContext : Context
    {

        internal MHRContext(IProcessManager process)
        {
            Game = new MHRGame(process);
        }

    }
}
