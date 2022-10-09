using HunterPie.UI.Architecture;
using System;

namespace HunterPie.GUI.Parts.Backup.ViewModels;
public class BackupElementViewModel : ViewModel
{
    private string _backupId;
    private string _gameName;
    private string _gameIcon;
    private DateTime _uploadedAt;
    private long _byteSize;
    private bool _isDownloading;

    public string BackupId { get => _backupId; set => SetValue(ref _backupId, value); }
    public string GameName { get => _gameName; set => SetValue(ref _gameName, value); }
    public string GameIcon { get => _gameIcon; set => SetValue(ref _gameIcon, value); }
    public DateTime UploadedAt { get => _uploadedAt; set => SetValue(ref _uploadedAt, value); }
    public long ByteSize { get => _byteSize; set => SetValue(ref _byteSize, value); }
    public bool IsDownloading { get => _isDownloading; set => SetValue(ref _isDownloading, value); }

}
