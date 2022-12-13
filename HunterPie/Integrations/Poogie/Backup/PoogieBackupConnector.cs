using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie.Backup.Models;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Backup;
internal class PoogieBackupConnector
{
    public static event EventHandler<BackupDeleteResponse>? OnBackupDeleted;

    private readonly PoogieConnector _connector = new();

    private const string BACKUP_ENDPOINT = "/v1/backup";

    public async Task<PoogieResult<CanUploadBackupResponse>> CanUploadBackup() =>
        await _connector.Get<CanUploadBackupResponse>(BACKUP_ENDPOINT + "/upload");

    public async Task<PoogieResult<UserBackupDetailsResponse>> FindAll() =>
        await _connector.Get<UserBackupDetailsResponse>(BACKUP_ENDPOINT);

    public async Task<PoogieResult<BackupResponse>> Upload(GameType gameType, string filename) =>
        await _connector.SendFile<BackupResponse>(BACKUP_ENDPOINT + $"/upload/{gameType}", filename);

    public async Task<HttpClientResponse> Download(string backupId) =>
        await Download(BACKUP_ENDPOINT + $"/{backupId}");

    public async Task<PoogieResult<BackupDeleteResponse>> Delete(string backupId)
    {
        PoogieResult<BackupDeleteResponse> result = await _connector.Delete<Nothing, BackupDeleteResponse>(BACKUP_ENDPOINT + $"/{backupId}", new Nothing());

        if (result.Response is { } resp)
            OnBackupDeleted?.Invoke(this, resp);

        return result;
    }
}
