using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Logging.Entity;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.UI.Logging.Services;

internal class HunterPieLogWriter : ILogWriter
{
    private readonly Dispatcher _dispatcher;
    private readonly ObservableCollection<LogString> _logs;

    public HunterPieLogWriter(
        Dispatcher dispatcher,
        ObservableCollection<LogString> logs)
    {
        _dispatcher = dispatcher;
        _logs = logs;
    }

    public void Benchmark(string message) =>
        WriteToBuffer(LogLevel.Benchmark, message);

    public void Debug(string message) =>
        WriteToBuffer(LogLevel.Debug, message);

    public void Native(string message) =>
        WriteToBuffer(LogLevel.Native, message);

    public void Info(string message) =>
        WriteToBuffer(LogLevel.Info, message);

    public void Warning(string message) =>
        WriteToBuffer(LogLevel.Warn, message);

    public void Error(string message) =>
        WriteToBuffer(LogLevel.Error, message);

    private void WriteToBuffer(LogLevel level, string message)
    {
        if (Application.Current is null)
            return;

        _dispatcher.Invoke(() =>
        {
            _logs.Add(
                item: new LogString
                {
                    Timestamp = $"[{DateTime.Now.ToLongTimeString()}]",
                    Message = message,
                    Level = level
                }
            );
        });
    }
}