﻿using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Events;
using HunterPie.Core.System.Windows;
using System;

namespace HunterPie.Core.System;

public static class ProcessManager
{
    public static event EventHandler<ProcessManagerEventArgs> OnProcessFound;
    public static event EventHandler<ProcessManagerEventArgs> OnProcessClosed;

    public static GameProcess Game { get; private set; } = GameProcess.None;

    public static IProcessManager[] Managers { get; } = new IProcessManager[]
    {
        new MHWProcessManager(),
        new MHRProcessManager(),
        new MHRSunbreakDemoProcessManager(),
    };

    public static void Start()
    {
        foreach (IProcessManager manager in Managers)
        {
            manager.OnGameStart += OnGameStartCallback;
            manager.OnGameClosed += OnGameClosedCallback;

            manager.Initialize();
        }
    }

    private static void OnGameClosedCallback(object sender, ProcessEventArgs e)
    {
        if (sender is IProcessManager manager)
        {
            ResumeAllPollingThreads(manager);
            Game = GameProcess.None;
            OnProcessClosed?.Invoke(sender, new(manager, e.ProcessName));
        }
    }

    private static void OnGameStartCallback(object sender, ProcessEventArgs e)
    {
        if (sender is IProcessManager manager)
        {
            PauseAllPollingThreads(manager);
            Game = manager.Game;
            OnProcessFound?.Invoke(sender, new((IProcessManager)sender, e.ProcessName));
        }
    }

    private static void PauseAllPollingThreads(IProcessManager activeManager)
    {
        foreach (IProcessManager manager in Managers)
        {
            if (manager != activeManager)
                manager.Pause();
        }
    }

    private static void ResumeAllPollingThreads(IProcessManager activeManager)
    {
        foreach (IProcessManager manager in Managers)
        {
            if (manager != activeManager)
                manager.Resume();
        }
    }
}
