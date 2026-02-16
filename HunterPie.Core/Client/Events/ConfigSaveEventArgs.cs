using System;

namespace HunterPie.Core.Client.Events;

public class ConfigSaveEventArgs(string path) : EventArgs
{

    public DateTime SyncedAt { get; } = DateTime.Now;
    public string Path { get; } = path;
}