using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Logger;
using HunterPie.GUI.Parts.Patches.ViewModels;
using HunterPie.Internal.Poogie;
using HunterPie.UI.Navigation;
using HunterPie.Update.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Update;

#nullable enable
internal static class UpdateUseCase
{
    private const string JUST_UPDATED_KEY = "JustUpdated";

    public static async Task<bool> Exec(UpdateViewModel vm)
    {
        vm.State = "Initializing HunterPie";
        UpdateService service = new();

        vm.State = "Checking for new localization files...";
        await service.UpdateLocalizationFiles();

        vm.State = "Checking for latest version...";
        Version? latest = await service.GetLatestVersion();

        if (latest is null || ClientInfo.IsVersionGreaterOrEq(latest))
            return false;

        vm.State = "New version found";

        if (ClientConfig.Config.Client.EnableAutoUpdateConfirmation)
        {
            NativeDialogResult result = DialogManager.Warn(
                Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='UPDATE_TITLE_STRING']"),
                Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='UPDATE_CONFIRMATION_DESCRIPTION_STRING']").Replace("{Latest}", $"{latest}"),
                NativeDialogButtons.Accept | NativeDialogButtons.Reject
            );

            if (result != NativeDialogResult.Accept)
                return false;
        }

        vm.State = "Downloading package...";
        bool success = await service.DownloadZip((_, args) =>
        {
            vm.DownloadedBytes = args.BytesDownloaded;
            vm.TotalBytes = args.TotalBytes;
        });

        if (!success)
        {
            vm.State = "Failed to update HunterPie";
            Log.Warn("Failed to update HunterPie");
            return false;
        }

        vm.State = "Extracting package...";
        if (!service.ExtractZip())
        {
            Log.Error("Failed to extract package");
            return false;
        }

        vm.State = "Calculating file hashes...";
        Dictionary<string, string>? remoteFiles = await service.TryIndexAllNewFilesRecursively();

        if (remoteFiles is not { })
            return false;

        Dictionary<string, string>? localFiles = await service.TryIndexLocalFilesFrom(remoteFiles);

        if (localFiles is not { })
            return false;

        vm.State = "Replacing old files";
        try
        {
            service.ReplaceOldFiles(localFiles, remoteFiles);
        }
        catch (Exception err)
        {
            RemoteCrashReporter.Send(err);

            string dialogMessage = err switch
            {
                IOException => "Failed to replace old files, make sure HunterPie has permissions to move files.",
                UnauthorizedAccessException => "HunterPie is missing permissions to manage files.",
                _ => "Failed to update HunterPie. Please check it is in a non-special folder and that it has permissions to write to files.",
            };

            DialogManager.Error("Update error", dialogMessage, NativeDialogButtons.Accept);

            Log.Error(err.ToString());
            return false;
        }

        RegistryConfig.Set(JUST_UPDATED_KEY, true);

        return true;
    }

    public static void OpenPatchNotesIfJustUpdated()
    {
        if (RegistryConfig.Exists(JUST_UPDATED_KEY) && !RegistryConfig.Get<bool>(JUST_UPDATED_KEY))
            return;

        Navigator.Body.Navigate(new PatchesViewModel());

        RegistryConfig.Set(JUST_UPDATED_KEY, false);
    }
}