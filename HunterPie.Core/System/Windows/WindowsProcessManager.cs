using System;
using System.Diagnostics;
using System.Threading;
using HunterPie.Core.Extensions;
using System.Linq;
using HunterPie.Core.Logger;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Address.Map;
using HunterPie.Core.System.Windows.Native;
using HunterPie.Core.System.Windows.Memory;
using HunterPie.Core.Events;
using System.Threading.Tasks;

namespace HunterPie.Core.System.Windows
{
    abstract class WindowsProcessManager : IProcessManager, IEventDispatcher
    {

        protected Thread pooler;
        private bool isProcessForeground;
        protected bool ShouldPollProcess = true;
       
        private IMemory memory; 
        private IntPtr pHandle;

        public event EventHandler<ProcessEventArgs> OnGameStart;
        public event EventHandler<ProcessEventArgs> OnGameClosed;
        public event EventHandler<ProcessEventArgs> OnGameFocus;
        public event EventHandler<ProcessEventArgs> OnGameUnfocus;

        public virtual string Name { get; }

        public int Version { get; private set; }
        public Process Process { get; private set; }
        public int ProcessId { get; private set; }
        public bool IsRunning { get; private set; }
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
                        new ProcessEventArgs(Name)
                        );
                }
            }
        }

        public IMemory Memory => memory;

        public void Initialize()
        {
            Log.Info($"Started scanning for process {Name}...");

            pooler = new Thread(new ThreadStart(ExecutePolling))
            {
                Name = "PollingBackgroundThread",
                IsBackground = true,
            };
            pooler.Start();
        }

        private void ExecutePolling()
        {
            while (ShouldPollProcess)
            {
                PollProcessInfo();
                Thread.Sleep(80);
            }
        }

        private void PollProcessInfo()
        {
            if (Process is not null)
            {
                IsProcessForeground = User32.GetForegroundWindow() == Process.MainWindowHandle;
                return;
            }

            Process mhProcess = Process.GetProcessesByName(Name)
                .FirstOrDefault(process => !string.IsNullOrEmpty(process?.MainWindowTitle));

            if (mhProcess is null)
                return;

            if (ShouldOpenProcess(mhProcess))
            {
                Process = mhProcess;
                ProcessId = mhProcess.Id;
                pHandle = Kernel32.OpenProcess(Kernel32.PROCESS_ALL_ACCESS, false, ProcessId);

                if (pHandle == IntPtr.Zero)
                {
                    Log.Error("Failed to open game process. Run HunterPie as Administrator!");
                    ShouldPollProcess = false;
                    return;
                }

                // Enable events from process
                Process.EnableRaisingEvents = true;
                Process.Exited += OnProcessExit;

                IsRunning = true;

                memory = new WindowsMemory(pHandle);

                AddressMap.Add("BASE", (long)Process.MainModule.BaseAddress);

                this.Dispatch(OnGameStart, new(Name));
            }
                
        }

        protected abstract bool ShouldOpenProcess(Process process);

        private void OnProcessExit(object sender, EventArgs e)
        {
            Process.Exited -= OnProcessExit;
            Process.Dispose();
            Process = null;

            Log.Info("Game process closed!");
            
            Kernel32.CloseHandle(pHandle);
            
            pHandle = IntPtr.Zero;

            this.Dispatch(OnGameClosed, new(Name));
        }

    }
}
