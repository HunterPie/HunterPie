using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Client
{
    public class Health : Scannable, IEventDispatcher
    {
        private readonly IProcessManager _process;

        public event EventHandler<EventArgs> OnHealthUpdate;
        public event EventHandler<EventArgs> OnMaxHealthUpdate;
        public event EventHandler<EventArgs> OnCriticalHealthUpdate;
        public event EventHandler<EventArgs> OnHeal;
        public event EventHandler<EventArgs> OnHealthExtStateUpdate;

        public Health(IProcessManager process)
        {
            _process = process;
        }


    }
}
