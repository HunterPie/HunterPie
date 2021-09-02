using HunterPie.Core.Domain.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Process
{
    public interface IProcessManager
    {
        public IMemory Memory { get; }

        public void Initialize();
    }
}
