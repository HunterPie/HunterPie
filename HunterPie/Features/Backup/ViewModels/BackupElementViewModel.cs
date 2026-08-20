using HunterPie.Core.Client;
using HunterPie.Core.Networking.Http.Models;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.UI.Architecture;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Features.Backup.ViewModels;

internal class BackupElementViewModel(
    PoogieBackupConnector backupConnector,
    INotificationService notificationService
) : ViewModel
{
    private readonly PoogieBackupConnector _backupConnector = backupConnector;

    public string BackupId { get; set => SetValue(ref field, value); } = string.Empty;
    public string GameName { get; set => SetValue(ref field, value); } = string.Empty;
    public string GameIcon { get; set => SetValue(ref field, value); } = string.Empty;
    public DateTime UploadedAt { get; set => SetValue(ref field, value); }
    public long ByteSize { get; set => SetValue(ref field, value); }
    public bool IsDownloading { get; set => SetValue(ref field, value); }
    public double BytesDownloaded { get; set => SetValue(ref field, value); }
    public double BytesToDownload { get; set => SetValue(ref field, value); }
    public bool IsDownloaded { get; set => SetValue(ref field, value); }

    public async Task Download()
    {
        IsDownloading = true;

        Response.Success response = await _backupConnector.DownloadAsync(BackupId);


        if (response.StatusCode is >= HttpStatusCode.BadRequest)
        {
            IsDownloading = false;
            return;
        }

        await response.DownloadAsync(
            outputPath: ClientInfo.GetPathFor($"Backups/{BackupId}.zip"),
            (e) =>
            {
                BytesDownloaded = e.DownloadedBytes;
                BytesToDownload = e.TotalBytes;
            }
        );

        IsDownloading = false;
        IsDownloaded = true;

        var options = new NotificationOptions(
            Type: NotificationType.Success,
            Title: "Download",
            Description: "Successfully downloaded backup",
            DisplayTime: TimeSpan.FromSeconds(5)
        );
        await notificationService.Show(options);
    }

    public void OpenBackupFolder()
    {
        Process.Start("explorer", ClientInfo.GetPathFor("Backups"));
    }

    public async Task Delete()
    {
        IsDownloading = true;
        await _backupConnector.DeleteAsync(BackupId);
        IsDownloading = false;
    }
}