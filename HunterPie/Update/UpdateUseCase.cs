using HunterPie.Core.Client;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Update.Presentation;
using System;
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

            bool isAutoUpdateEnabled = ClientConfig.Config.Client.EnableAutoUpdate;
            
            if (!isAutoUpdateEnabled)
                return false;

            vm.State = "Checking for latest version...";
            Version latest = await service.GetLatestVersion();

            if (latest is null || ClientInfo.IsVersionGreaterOrEq(latest))
                return false;

            vm.State = "New version found";
            var result = DialogManager.Warn(
                "Update",
                "There's a new version of HunterPie.\nDo you want to update now?",
                NativeDialogButtons.Accept | NativeDialogButtons.Reject
            );

            if (result != NativeDialogResult.Accept)
                return false;

            vm.State = "Downloading package...";
            await service.DownloadZip((_, args) =>
            {
                vm.DownloadedBytes = args.BytesReceived;
                vm.TotalBytes = args.TotalBytesToReceive;
            });

            vm.State = "Calculating file hashes...";
            var localFiles = await service.IndexAllFilesRecursively(ClientInfo.ClientPath);

            vm.State = "Extracting package...";
            service.ExtractZip();
            var remoteFiles = await service.IndexAllFilesRecursively(ClientInfo.GetPathFor(@"temp/HunterPie"));

            vm.State = "Replacing old files";
            service.ReplaceOldFiles(localFiles, remoteFiles);

            return true;
        }
    }
}
