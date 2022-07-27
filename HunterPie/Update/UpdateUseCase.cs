using HunterPie.Core.Client;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Logger;
using HunterPie.Internal.Poogie;
using HunterPie.Update.Presentation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Update
{
    internal static class UpdateUseCase
    {
        public static async Task<bool> Exec(UpdateViewModel vm)
        {
            vm.State = "Initializing HunterPie";
            UpdateService service = new();
            service.CleanupOldFiles();

            vm.State = "Checking for latest version...";
            Version latest = await service.GetLatestVersion();

            if (latest is null || ClientInfo.IsVersionGreaterOrEq(latest))
                return false;

            vm.State = "New version found";

            if (ClientConfig.Config.Client.EnableAutoUpdateConfirmation)
            {
                var result = DialogManager.Warn(
                    "Update",
                    $"Version v{latest} is now available.\nDo you want to update now?",
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
            var localFiles = await service.IndexAllFilesRecursively(ClientInfo.ClientPath);

            vm.State = "Extracting package...";
            if (!service.ExtractZip())
            {
                Log.Error("Failed to extract package");
                return false;
            }
            var remoteFiles = await service.IndexAllFilesRecursively(ClientInfo.GetPathFor(@"temp/HunterPie"));

            vm.State = "Replacing old files";
            try
            {
                service.ReplaceOldFiles(localFiles, remoteFiles);
            } catch(Exception err)
            {
                RemoteCrashReporter.Send(err);

                string dialogMessage = err switch
                {
                    IOException => "Failed to replace old files, make sure HunterPie has permissions to move files.",
                    UnauthorizedAccessException => "HunterPie is missing permissions to manage files.",
                    _ => null,
                };

                if (dialogMessage is string message)
                    DialogManager.Error("Update error", message, NativeDialogButtons.Accept);

                Log.Error(err.ToString());
            }

            return true;
        }
    }
}
