using System;

namespace HunterPie.Core.Networking.Http.Events;

public class DownloadEventArgs(long bytesDownloaded, long totalBytes) : EventArgs
{

    public long BytesDownloaded { get; } = bytesDownloaded;
    public long TotalBytes { get; } = totalBytes;
}