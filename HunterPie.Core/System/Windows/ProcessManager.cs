using HunterPie.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HunterPie.Core.Extensions;
using System.Linq;
using HunterPie.Core.Logger;

namespace HunterPie.Core.System.Windows
{
    public class ProcessManager : IEventDispatcher
    {

        private Timer pooler;
        private bool isProcessForeground;
        
        public const string Name = "MonsterHunterWorld";
        public int Version { get; private set; }
        public Process Process { get; private set; }
        public int ProcessId { get; private set; }
        public bool IsProcessForeground
        {
            get => isProcessForeground;
            private set
            {
                if (isProcessForeground != value)
                {
                    isProcessForeground = value;

                    this.Dispatch(
                        value ? OnGameFocus 
                              : OnGameUnfocus, 
                        EventArgs.Empty
                        );
                }
            }
        }

        public event EventHandler<EventArgs> OnGameStart;
        public event EventHandler<EventArgs> OnGameClosed;
        public event EventHandler<EventArgs> OnGameFocus;
        public event EventHandler<EventArgs> OnGameUnfocus;

        private void SetupScanning()
        {
            pooler = new Timer(delegate { PoolProcessInfo(); } , null, 0, 80);
        }

        private void PoolProcessInfo()
        {
            if (Process != null)
                return;

            Process monsterHunterProcess = Process.GetProcessesByName(Name)
                .FirstOrDefault(process => !string.IsNullOrEmpty(process.MainWindowTitle));

            if (monsterHunterProcess != null)
            {
                // If our process is in either another window, or not initialized yet
                if (!monsterHunterProcess.MainWindowTitle.ToUpper().StartsWith("MONSTER HUNTER: WORLD"))
                    return;

                Process = monsterHunterProcess;
                ProcessId = Process.Id;

                try
                {
                    Version = int.Parse(monsterHunterProcess.MainWindowTitle.Split('(')[1].Trim(')'));
                } catch
                {
                    Log.Error("Failed to get Monster Hunter: World build version. Loading latest map version instead.");
                }
            }
        }
    }
}
