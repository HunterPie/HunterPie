using HunterPie.Core.Logger;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HunterPie.Internal.Logger
{
    internal class NativeLogger : ILogger
    {
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AllocConsole();

        private object synchronizer = new();

        private enum LogLevel
        {
            Debug = ConsoleColor.Green,
            Warn = ConsoleColor.Yellow,
            Error = ConsoleColor.Red,
            Info = ConsoleColor.Cyan
        }

        public NativeLogger()
        {
            Process currentProcess = Process.GetCurrentProcess();
            AllocConsole();
            Console.Title = "[DEBUG] HunterPie Console";
        }

        public Task Debug(params object[] args)
        {
            return WriteToStdout(LogLevel.Debug, args);
        }

        public Task Error(params object[] args)
        {
            return WriteToStdout(LogLevel.Error, args);
        }

        public Task Info(params object[] args)
        {
            return WriteToStdout(LogLevel.Info, args);
        }

        public Task Warn(params object[] args)
        {
            return WriteToStdout(LogLevel.Warn, args);
        }

        private Task WriteToStdout(LogLevel level, params object[] args)
        {
            return Task.Run(() =>
            {
                lock (synchronizer)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"[{DateTime.Now.ToLongTimeString()}] ");
                    Console.ForegroundColor = (ConsoleColor)level;
                    Console.Write($"[{level.ToString().ToUpper()}] ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    foreach (object arg in args)
                        Console.Write(arg.ToString());
                    Console.Write("\n");
                }
            });
        }
    }
}
