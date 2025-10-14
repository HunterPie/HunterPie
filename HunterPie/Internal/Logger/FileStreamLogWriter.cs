using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Observability.Logging;
using System;
using System.IO;
using System.Text;

namespace HunterPie.Internal.Logger;

internal class FileStreamLogWriter : ILogWriter
{
    private bool _isClosed = false;
    private static readonly FileStream Stream = new(ClientInfo.GetPathFor("HunterPie_Log.txt"), FileMode.Create, FileAccess.Write, FileShare.Read, 4096);

    public void Debug(string message) =>
        WriteToBuffer(LogLevel.Debug, message);

    public void Info(string message) =>
        WriteToBuffer(LogLevel.Info, message);

    public void Warning(string message) =>
        WriteToBuffer(LogLevel.Warn, message);

    public void Native(string message) =>
        WriteToBuffer(LogLevel.Info, message);

    public void Error(string message) =>
        WriteToBuffer(LogLevel.Error, message);

    public void Benchmark(string message) =>
        WriteToBuffer(LogLevel.Info, message);


    private void WriteToBuffer(LogLevel level, string message)
    {
        if (_isClosed)
            return;

        string now = DateTime.Now.ToLongTimeString();
        byte[] buffer = Encoding.UTF8.GetBytes($"[{now}][{level}] {message}\n");
        Stream.Write(buffer, 0, buffer.Length);
        Stream.Flush();
    }

    public void Dispose()
    {
        _isClosed = true;
        Stream.Dispose();
    }
}