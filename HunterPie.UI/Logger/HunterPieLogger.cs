using HunterPie.Core.Logger;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.UI.Logger
{
    public class HunterPieLogger : ILogger
    {

        public static readonly ObservableCollection<LogString> ViewModel = new ObservableCollection<LogString>();

        public Task Benchmark(params object[] args)
        {
            return WriteToBuffer(LogLevel.Benchmark, args);
        }

        public Task Debug(params object[] args)
        {
            return WriteToBuffer(LogLevel.Debug, args);
        }

        public Task Error(params object[] args)
        {
            return WriteToBuffer(LogLevel.Error, args);
        }

        public Task Info(params object[] args)
        {
            return WriteToBuffer(LogLevel.Info, args);
        }

        public Task Warn(params object[] args)
        {
            return WriteToBuffer(LogLevel.Warn, args);
        }

        private async Task WriteToBuffer(LogLevel level, params object[] args)
        {
            try
            {
                await Application.Current?.Dispatcher?.InvokeAsync(() =>
                {
                    StringBuilder builder = new StringBuilder();

                    foreach (object e in args)
                        builder.Append(e.ToString());

                    ViewModel.Add(new LogString
                    {
                        Timestamp = $"[{DateTime.Now.ToLongTimeString()}]",
                        Message = builder.ToString(),
                        Level = level
                    });

                });
            } catch {}
        }
    }
}
