using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Backups.Games;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.Integrations.Poogie.Backup.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Features.Backups;

#nullable enable
internal class GameSaveBackupService : IContextInitializer
{
    private static readonly PoogieBackupConnector BackupConnector = new();

    private const string LAST_BACKUP_KEY = "HUNTERPIE_LAST_BACKUP";
    private const string LAST_BACKUP_SUCCESS_KEY = "HUNTERPIE_LAST_BACKUP_SUCCESS";
    private static IBackupService? _backupService;

    public Task InitializeAsync(IContext context)
    {
        return Task.Factory.StartNew(() =>
        {
            string? steamSavePath = GetSteamSaveFolder();

            if (steamSavePath is null)
                return;

            _backupService = context switch
            {
                MHRContext => new MHRBackupService(steamSavePath),
                MHWContext => new MHWBackupService(steamSavePath),
                _ => null
            };
        });
    }

    public static async Task<bool> ExecuteBackup()
    {
        bool isLoggedIn = await AccountManager.ValidateSessionToken();

        if (!isLoggedIn)
            return false;

        if (!LocalAccountConfig.Config.IsBackupEnabled)
            return false;

        PoogieResult<CanUploadBackupResponse> canUploadResult = await BackupConnector.CanUploadBackup();

        if (canUploadResult.Response is not { CanUpload: true })
            return false;

        string registryKey = $"{LAST_BACKUP_KEY}_{_backupService!.Type}";
        string successRegistryKey = $"{LAST_BACKUP_SUCCESS_KEY}_{_backupService!.Type}";

        if (!ShouldBackup())
            return false;

        string backupFile = await _backupService.ExecuteAsync();

        if (backupFile is null)
            return false;

        PoogieResult<BackupResponse> result = await BackupConnector.Upload(_backupService.Type, backupFile);

        if (File.Exists(backupFile))
            try
            {
                File.Delete(backupFile);
            }
            catch { }

        if (result.Response is null)
            return false;

        Log.Debug("Successfully uploaded save file {0}", result.Response!.Id);

        RegistryConfig.Set(registryKey, DateTime.UtcNow.Ticks);

        RegistryConfig.Set(successRegistryKey, true);

        return true;
    }

    private static bool ShouldBackup()
    {
        string registryKey = $"{LAST_BACKUP_KEY}_{_backupService!.Type}";

        if (!RegistryConfig.Exists(registryKey))
            return true;

        // TODO: Remove this in the future as it's only a temporary workaround to fix
        // cultural DateTime parsing
        long lastBackupTicks;
        try
        {
            lastBackupTicks = RegistryConfig.Get<long>(registryKey);
        }
        catch
        {
            RegistryConfig.Delete(registryKey);
            return true;
        }

        double timeSinceLastBackup = (DateTime.UtcNow - new DateTime(lastBackupTicks)).TotalHours;

        return timeSinceLastBackup >= 23;
    }

    private string? GetSteamSaveFolder()
    {
        using RegistryKey? activeProcess = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam\ActiveProcess");

        if (activeProcess is null)
            return null;

        int? activeUser = (int?)activeProcess.GetValue("ActiveUser");
        string? steamClientPath = (string?)activeProcess.GetValue("SteamClientDll");

        if (activeUser is null || steamClientPath is null)
            return null;

        string steamPath = Path.GetDirectoryName(steamClientPath)!;

        return Path.Combine(steamPath, "userdata", activeUser.Value.ToString());
    }
}
