using HunterPie.Core.Domain.Process;
using System;

namespace HunterPie.Core.System
{
    public class ProcessManagerEventArgs : EventArgs
    {

        public IProcessManager Process { get; }
        public string ProcessName { get; }

        public ProcessManagerEventArgs(IProcessManager process, string processName)
        {
            Process = process;
            ProcessName = processName;
        }

    }
}
