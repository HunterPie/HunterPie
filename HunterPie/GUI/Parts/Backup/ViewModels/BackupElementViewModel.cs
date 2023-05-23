using HunterPie.Core.Client;
using HunterPie.Core.Networking.Http;
using HunterPie.Core.Notification;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.UI.Architecture;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Backup.ViewModels;
public class BackupElementViewModel : ViewModel
{
    private readonly PoogieBackupConnector _backupConnector = new();

    private string _backupId = string.Empty;
    private string _gameName = string.Empty;
    private string _gameIcon = string.Empty;
    private DateTime _uploadedAt;
    private long _byteSize;
    private bool _isDownloading;
    private double _bytesDownloaded;
    private double _bytesToDownload;
    private bool _isDownloaded;

    public string BackupId { get => _backupId; set => SetValue(ref _backupId, value); }
    public string GameName { get => _gameName; set => SetValue(ref _gameName, value); }
    public string GameIcon { get => _gameIcon; set => SetValue(ref _gameIcon, value); }
    public DateTime UploadedAt { get => _uploadedAt; set => SetValue(ref _uploadedAt, value); }
    public long ByteSize { get => _byteSize; set => SetValue(ref _byteSize, value); }
    public bool IsDownloading { get => _isDownloading; set => SetValue(ref _isDownloading, value); }
    public double BytesDownloaded { get => _bytesDownloaded; set => SetValue(ref _bytesDownloaded, value); }
    public double BytesToDownload { get => _bytesToDownload; set => SetValue(ref _bytesToDownload, value); }
    public bool IsDownloaded { get => _isDownloaded; set => SetValue(ref _isDownloaded, value); }

    public async Task Download()
    {
        IsDownloading = true;

        using HttpClientResponse response = await _backupConnector.Download(BackupId);


        if (!response.Success)
        {
            IsDownloading = false;
            return;
        }

        response.OnDownloadProgressChanged += (_, args) =>
        {
            BytesDownloaded = args.BytesDownloaded;
            BytesToDownload = args.TotalBytes;
        };

        await response.DownloadAsync(
            ClientInfo.GetPathFor($"Backups/{BackupId}.zip")
        );

        IsDownloading = false;
        IsDownloaded = true;

        NotificationService.Success(
            "Successfully downloaded backup",
            TimeSpan.FromSeconds(5)
        );
    }

    public void OpenBackupFolder()
    {
        Process.Start("explorer", ClientInfo.GetPathFor("Backups"));
    }

    public async Task Delete()
    {
        IsDownloading = true;
        await _backupConnector.Delete(BackupId);
        IsDownloading = false;
    }
}
