using System;

namespace HunterPie.Core.Http.Events
{
    public class PoogieDownloadEventArgs : EventArgs
    {
        public long BytesDownloaded { get; }
        public long TotalBytes { get; }

        public PoogieDownloadEventArgs(long bytesDownloaded, long totalBytes)
        {
            BytesDownloaded = bytesDownloaded;
            TotalBytes = totalBytes;
        }
    }
}
