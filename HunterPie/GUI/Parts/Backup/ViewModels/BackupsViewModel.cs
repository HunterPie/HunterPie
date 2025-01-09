using HunterPie.Core.Client;
using HunterPie.Features.Account.Config;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.Integrations.Poogie.Backup.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace HunterPie.GUI.Parts.Backup.ViewModels;

public class BackupsViewModel : ViewModel, IDisposable
{
    private readonly PoogieBackupConnector _backupConnector = new();

    private int _count;
    private int _maxCount;
    private DateTime _lastSync;
    private bool _isFetching;
    private bool _hasBackups;

    public ObservableCollection<BackupElementViewModel> Backups { get; } = new();
    public int Count { get => _count; set => SetValue(ref _count, value); }
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }
    public DateTime LastSync { get => _lastSync; set => SetValue(ref _lastSync, value); }
    public bool IsFetching { get => _isFetching; set => SetValue(ref _isFetching, value); }
    public bool HasBackups { get => _hasBackups; set => SetValue(ref _hasBackups, value); }
    public bool IsBackupEnabled => LocalAccountConfig.Config.IsBackupEnabled.Value;

    public async void FetchBackups()
    {
        IsFetching = true;
        PoogieResult<UserBackupDetailsResponse> backupDetails = await _backupConnector.FindAll();
        IsFetching = false;

        if (backupDetails.Response is null)
            return;

        UserBackupDetailsResponse response = backupDetails.Response;

        Count = response.Count;
        MaxCount = response.MaxCount;
        HasBackups = response.Backups.Length > 0;
        LastSync = response.Backups.FirstOrDefault()?.UploadedAt.ToLocalTime() ?? DateTime.MinValue;

        await UIThread.InvokeAsync(() =>
        {
            foreach (BackupResponse backup in response.Backups)
                Backups.Add(new BackupElementViewModel
                {
                    BackupId = backup.Id,
                    GameName = backup.GameName,
                    GameIcon = backup.GameIcon,
                    ByteSize = backup.Size,
                    UploadedAt = backup.UploadedAt.ToLocalTime(),
                    IsDownloaded = File.Exists(
                        ClientInfo.GetPathFor($"Backups/{backup.Id}.zip")
                    )
                });
        });
    }

    private async void OnBackupDeleted(object? sender, BackupDeleteResponse e)
    {
        await UIThread.InvokeAsync(() =>
        {
            BackupElementViewModel backup = Backups.First(vm => vm.BackupId == e.Id);

            Backups.Remove(backup);
        });

        Count = Backups.Count;
    }

    public void Dispose() => UnhookEvents();
}