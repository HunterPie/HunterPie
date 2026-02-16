using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.Integrations.Poogie.Backup.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Backup.ViewModels;

internal class BackupsViewModel(
    PoogieBackupConnector backupConnector,
    AccountConfig accountConfig,
    BackupElementFactory elementFactory) : ViewModel
{
    private readonly PoogieBackupConnector _backupConnector = backupConnector;
    private readonly AccountConfig _accountConfig = accountConfig;
    private readonly BackupElementFactory _elementFactory = elementFactory;

    public ObservableCollection<BackupElementViewModel> Backups { get; } = new();
    public int Count { get; set => SetValue(ref field, value); }
    public int MaxCount { get; set => SetValue(ref field, value); }
    public DateTime LastSync { get; set => SetValue(ref field, value); }
    public bool IsFetching { get; set => SetValue(ref field, value); }
    public bool HasBackups { get; set => SetValue(ref field, value); }

    public bool IsBackupEnabled => _accountConfig.IsBackupEnabled.Value;

    public async Task FetchBackupsAsync()
    {
        IsFetching = true;
        PoogieResult<UserBackupDetailsResponse> backupDetails = await _backupConnector.FindAllAsync();
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
            response.Backups
                .Select(_elementFactory.Create)
                .ForEach(Backups.Add);
        });
    }

    private async void OnBackupDeleted(object? _, BackupDeleteResponse e)
    {
        await UIThread.InvokeAsync(() =>
        {
            BackupElementViewModel backup = Backups.First(vm => vm.BackupId == e.Id);

            Backups.Remove(backup);
        });

        Count = Backups.Count;
    }
}