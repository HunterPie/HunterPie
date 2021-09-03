using HunterPie.Core.Domain.Memory;
using System;

namespace HunterPie.Core.Domain.Process
{
    public interface IProcessManager
    {

        public event EventHandler<EventArgs> OnGameStart;
        public event EventHandler<EventArgs> OnGameClosed;
        public event EventHandler<EventArgs> OnGameFocus;
        public event EventHandler<EventArgs> OnGameUnfocus;

        public IMemory Memory { get; }

        public void Initialize();
    }
}
