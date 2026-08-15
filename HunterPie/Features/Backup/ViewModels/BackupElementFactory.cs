using HunterPie.Core.Client;
using HunterPie.Core.Notification;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.Integrations.Poogie.Backup.Models;
using System.IO;

namespace HunterPie.Features.Backup.ViewModels;

internal class BackupElementFactory(
    PoogieBackupConnector backupConnector,
    INotificationService notificationService
)
{
    public BackupElementViewModel Create(BackupResponse response) =>
        new(
            backupConnector: backupConnector,
            notificationService: notificationService
        )
        {
            BackupId = response.Id,
            GameName = response.GameName,
            GameIcon = response.GameIcon,
            ByteSize = response.Size,
            UploadedAt = response.UploadedAt.ToLocalTime(),
            IsDownloaded = File.Exists(
                ClientInfo.GetPathFor($"Backups/{response.Id}.zip")
            )
        };
}