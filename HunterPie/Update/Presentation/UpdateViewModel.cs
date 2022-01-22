using HunterPie.Core.Architecture;

namespace HunterPie.Update.Presentation
{
    internal class UpdateViewModel : Bindable
    {
        private long _downloadedBytes;
        private long _totalBytes;
        private double _downloadPercentage;
        private string _state;

        public long DownloadedBytes { get => _downloadedBytes; set { SetValue(ref _downloadedBytes, value); } }
        public long TotalBytes { get => _totalBytes; set { SetValue(ref _totalBytes, value); } }
        public double DownloadedPercentage { get => _downloadPercentage; set { SetValue(ref _downloadPercentage, value); } }
        public string State { get => _state; set { SetValue(ref _state, value); } }
    }
}
