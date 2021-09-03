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
using System.IO;
using HunterPie.Core.System.Windows.Memory;

namespace HunterPie.Core.System.Windows
{
    public class WindowsProcessManager : IProcessManager, IEventDispatcher
    {

        private Timer pooler;
        private bool isProcessForeground;
       
        private IMemory memory; 
        private IntPtr pHandle;

        public const string Name = "MonsterHunterWorld";

        public event EventHandler<EventArgs> OnGameStart;
        public event EventHandler<EventArgs> OnGameClosed;
        public event EventHandler<EventArgs> OnGameFocus;
        public event EventHandler<EventArgs> OnGameUnfocus;

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
                        EventArgs.Empty
                        );
                }
            }
        }

        public IMemory Memory => memory;

        public void Initialize()
        {
            Log.Info("Started scanning for Monster Hunter: World process...");
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
                pHandle = Kernel32.OpenProcess(Kernel32.PROCESS_ALL_ACCESS, false, ProcessId);

                if (pHandle == IntPtr.Zero)
                {
                    Log.Error("Failed to open game process. Run HunterPie as Administrator!");
                    pooler.Dispose();
                    return;
                }

                string version = monsterHunterProcess.MainWindowTitle.Split('(')[1].Trim(')');
                bool parsed = int.TryParse(version, out int parsedVersion);

                string mapPath = Path.Combine(AppContext.BaseDirectory, "address");

                if (!parsed)
                {
                    Log.Error("Failed to get Monster Hunter: World build version. Loading latest map version instead."); 
                    AddressMap.ParseLatest(mapPath);
                } else
                {
                    AddressMap.Parse(Path.Combine(mapPath, $"MonsterHunterWorld.{version}.map"));
                }
                // Enable events from process
                Process.EnableRaisingEvents = true;
                Process.Exited += OnProcessExit;

                IsRunning = true;
                
                memory = new WindowsMemory(pHandle);

                this.Dispatch(OnGameStart);
            }
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Process.Exited -= OnProcessExit;
            Process.Dispose();
            Process = null;

            Log.Info("Game process closed!");
            
            Kernel32.CloseHandle(pHandle);
            
            pHandle = IntPtr.Zero;

            this.Dispatch(OnGameClosed);
        }
    }
}
