using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Logger;
using HunterPie.Internal.Poogie;
using HunterPie.Update.Presentation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Update;

internal static class UpdateUseCase
{
    public static async Task<bool> Exec(UpdateViewModel vm)
    {
        vm.State = "Initializing HunterPie";
        UpdateService service = new();
        _ = service.CleanupOldFiles();

        vm.State = "Checking for latest version...";
        Version latest = await service.GetLatestVersion();

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
        await service.DownloadZip((_, args) =>
        {
            vm.DownloadedBytes = args.BytesDownloaded;
            vm.TotalBytes = args.TotalBytes;
        });

        vm.State = "Calculating file hashes...";
        System.Collections.Generic.Dictionary<string, string> localFiles = await service.IndexAllFilesRecursively(ClientInfo.ClientPath);

        vm.State = "Extracting package...";
        if (!service.ExtractZip())
        {
            Log.Error("Failed to extract package");
            return false;
        }

        System.Collections.Generic.Dictionary<string, string> remoteFiles = await service.IndexAllFilesRecursively(ClientInfo.GetPathFor(@"temp/HunterPie"));

        vm.State = "Replacing old files";
        try
        {
            _ = service.ReplaceOldFiles(localFiles, remoteFiles);
        }
        catch (Exception err)
        {
            RemoteCrashReporter.Send(err);

            string dialogMessage = err switch
            {
                IOException => "Failed to replace old files, make sure HunterPie has permissions to move files.",
                UnauthorizedAccessException => "HunterPie is missing permissions to manage files.",
                _ => null,
            };

            if (dialogMessage is string message)
                _ = DialogManager.Error("Update error", message, NativeDialogButtons.Accept);

            Log.Error(err.ToString());
        }

        return true;
    }
}
