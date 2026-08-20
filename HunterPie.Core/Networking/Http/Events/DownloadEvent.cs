namespace HunterPie.Core.Networking.Http.Events;

public delegate void DownloadEventHandler(DownloadEvent e);

public record class DownloadEvent(
    bool IsLengthUnknown,
    long DownloadedBytes,
    long TotalBytes
);
