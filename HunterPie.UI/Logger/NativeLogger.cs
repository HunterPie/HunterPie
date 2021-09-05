using HunterPie.Core.Logger;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.UI.Logger
{
    internal class NativeLogger : ILogger
    {

        public static readonly ObservableCollection<LogString> viewModel = new ObservableCollection<LogString>();

        public Task Debug(params object[] args)
        {
            return WriteToBuffer("#000000", args);
        }

        public Task Error(params object[] args)
        {
            return WriteToBuffer("#000000", args);
        }

        public Task Info(params object[] args)
        {
            return WriteToBuffer("#000000", args);
        }

        public Task Warn(params object[] args)
        {
            return WriteToBuffer("#000000", args);
        }

        private async Task WriteToBuffer(string level, params object[] args)
        {
            DateTime timestamp = DateTime.Now;
            
            StringBuilder message = new StringBuilder($"[{timestamp.ToLongTimeString()}]");
            message.AppendJoin(" ", args);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                viewModel.Add(new LogString
                {
                    Message = message.ToString(),
                    Color = level
                });
            });

            
        }
    }
}
