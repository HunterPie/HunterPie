using System;

namespace HunterPie.Core.Events
{
    public class ProcessEventArgs : EventArgs
    {

        public string ProcessName { get; }

        public ProcessEventArgs(string name)
        {
            ProcessName = name;
        }
    }
}
