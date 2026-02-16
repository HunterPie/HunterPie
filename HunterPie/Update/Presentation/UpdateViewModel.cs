using HunterPie.UI.Architecture;

namespace HunterPie.Update.Presentation;

internal class UpdateViewModel : ViewModel
{
    public long DownloadedBytes { get; set => SetValue(ref field, value); }
    public long TotalBytes { get; set => SetValue(ref field, value); }
    public string State { get; set => SetValue(ref field, value); } = string.Empty;
}