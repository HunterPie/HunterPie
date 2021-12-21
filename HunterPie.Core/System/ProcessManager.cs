using HunterPie.Core.Domain.Process;
using HunterPie.Core.Events;
using HunterPie.Core.System.Windows;
using System;

namespace HunterPie.Core.System
{
    public static class ProcessManager
    {

        public static event EventHandler<ProcessManagerEventArgs> OnProcessFound;
        public static event EventHandler<ProcessManagerEventArgs> OnProcessClosed;

        private readonly static IProcessManager[] _managers = new[]
        {
            new MHWProcessManager()
        };
        public static IProcessManager[] Managers => _managers;

        public static void Start()
        {
            foreach (IProcessManager manager in Managers)
            {
                manager.OnGameStart += OnGameStartCallback;
                manager.OnGameClosed += OnGameClosedCallback;

                manager.Initialize();
            }
        }

        private static void OnGameClosedCallback(object sender, ProcessEventArgs e) =>
            OnProcessClosed?.Invoke(sender, new((IProcessManager)sender, e.ProcessName));

        private static void OnGameStartCallback(object sender, ProcessEventArgs e) =>
            OnProcessFound?.Invoke(sender, new((IProcessManager)sender, e.ProcessName));
    }
}
