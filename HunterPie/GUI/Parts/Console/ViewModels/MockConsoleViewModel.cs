using HunterPie.UI.Logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Console.ViewModels
{
    internal class MockConsoleViewModel
    {
        public ObservableCollection<LogString> Logs { get; } = new();

        public MockConsoleViewModel()
        {
            Logs.Add(new()
            {
                Message = "This is a debug log",
                Timestamp = "[14:00:00]",
                Level = LogLevel.Debug
            });

            Logs.Add(new()
            {
                Message = "This is a benchmark log",
                Timestamp = "[14:00:01]",
                Level = LogLevel.Benchmark
            });

            Logs.Add(new()
            {
                Message = "This is an info log",
                Timestamp = "[14:00:02]",
                Level = LogLevel.Info
            });

            Logs.Add(new()
            {
                Message = "This is a warn log",
                Timestamp = "[14:00:03]",
                Level = LogLevel.Warn
            });

            Logs.Add(new()
            {
                Message = "This is an error log",
                Timestamp = "[14:01:56]",
                Level = LogLevel.Error
            });

        }
    }
}
