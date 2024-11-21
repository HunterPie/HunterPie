using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Events;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Windows.Memory;
using HunterPie.Core.System.Windows.Native;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace HunterPie.Core.System.Windows;

internal abstract class WindowsProcessManager : IProcessManager, IEventDispatcher
{

    protected Thread? _pooler;
    private bool _isProcessForeground;
    private bool _shouldPauseThread;
    protected bool ShouldPollProcess = true;

    private IMemory? _memory;
    private IntPtr pHandle;
    private ProcessStatus _status = ProcessStatus.NotFound;

    public event EventHandler<ProcessEventArgs>? OnGameStart;
    public event EventHandler<ProcessEventArgs>? OnGameClosed;
    public event EventHandler<ProcessEventArgs>? OnGameFocus;
    public event EventHandler<ProcessEventArgs>? OnGameUnfocus;
    public event EventHandler<SimpleValueChangeEventArgs<ProcessStatus>>? OnProcessStatusChange;

    public abstract string Name { get; }
    public abstract GameProcess Game { get; }

    /// <inheritdoc />
    public bool? HasExitedNormally { get; private set; }

    public int Version { get; private set; }

    public Process? Process { get; private set; }

    public int ProcessId { get; private set; }

    public bool IsRunning { get; private set; }

    public bool IsProcessForeground
    {
        get => _isProcessForeground;
        private set
        {
            if (_isProcessForeground != value)
            {
                _isProcessForeground = value;

                this.Dispatch(
                    value ? OnGameFocus
                          : OnGameUnfocus,
                    new ProcessEventArgs(Name)
                );
            }
        }
    }

    public ProcessStatus Status
    {
        get => _status;
        protected set
        {
            if (value == _status)
                return;

            ProcessStatus temp = _status;
            _status = value;
            this.Dispatch(OnProcessStatusChange, new SimpleValueChangeEventArgs<ProcessStatus>(temp, _status));
        }
    }

    public IMemory Memory => _memory!;

    public void Initialize()
    {
        Log.Info($"Started scanning for process {Name}...");

        _pooler = new Thread(new ThreadStart(ExecutePolling)) { Name = "PollingBackgroundThread", IsBackground = true, };
        _pooler.Start();
    }

    private void ExecutePolling()
    {
        while (ShouldPollProcess)
        {
            if (_shouldPauseThread)
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException)
                {
                    continue;
                }

            PollProcessInfo();
            Thread.Sleep(150);
        }
    }

    private void PollProcessInfo()
    {
        if (Process?.HasExited == true)
        {
            OnProcessExit();
            return;
        }

        if (Process is not null)
        {
            IsProcessForeground = User32.GetForegroundWindow() == Process.MainWindowHandle;
            return;
        }

        Process? mhProcess = Process.GetProcessesByName(Name)
            .FirstOrDefault(process => !string.IsNullOrEmpty(process.MainWindowTitle));

        if (mhProcess is null)
            return;

        if (!ShouldOpenProcess(mhProcess))
            return;

        try
        {
            if (mhProcess.MainModule is null)
                throw new InvalidOperationException("Process main module is null.");

            Process = mhProcess;
            ProcessId = mhProcess.Id;
            HasExitedNormally = null;
            // We want to retrieve process exit code, so force Process to call OpenProcess by explicitly retrieving its SafeHandle.
            // Otherwise there will be InvalidOperationException: Process was not started by this object, so requested information cannot be determined.
            _ = Process.SafeHandle;
            pHandle = Kernel32.OpenProcess(Kernel32.PROCESS_ALL_ACCESS, false, ProcessId);

            if (pHandle == IntPtr.Zero)
                throw new Win32Exception();

            IsRunning = true;

            _memory = new WindowsMemory(pHandle);

            AddressMap.Add("BASE", (long)Process.MainModule.BaseAddress);

            this.Dispatch(OnGameStart, new(Name));
            Status = ProcessStatus.Hooked;
        }
        catch (Exception ex)
        {
            Log.Error("Failed to open game process. Run HunterPie as Administrator!");
            Log.Info("Error details: {0}", ex);
            ShouldPollProcess = false;
        }
    }

    protected abstract bool ShouldOpenProcess(Process process);

    private void OnProcessExit()
    {
        Debug.Assert(Process != null);

        HasExitedNormally = Process.ExitCode == 0;
        Process.Dispose();
        Process = null;

        _ = Kernel32.CloseHandle(pHandle);

        pHandle = IntPtr.Zero;

        this.Dispatch(OnGameClosed, new(Name));
        Status = ProcessStatus.Waiting;
    }

    public void Pause()
    {
        _shouldPauseThread = true;

        if (Status != ProcessStatus.Hooked)
            Status = ProcessStatus.Paused;
    }

    public void Resume()
    {
        _shouldPauseThread = false;
        _pooler?.Interrupt();

        Status = ProcessStatus.Waiting;
    }

}
