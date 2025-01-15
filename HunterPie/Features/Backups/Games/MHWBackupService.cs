using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using HunterPie.Core.Zip;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Backups.Games;
internal class MHWBackupService : IBackupService
{
    private readonly string _savesPath;
    private readonly string[] _filesPattern = { "*.dat", "SAVEDATA*" };
    public GameType Type => GameType.MHW;

    private string WorldSaveFolder => _savesPath + "/582010/remote";

    public MHWBackupService(string savesPath)
    {
        _savesPath = savesPath;
    }

    public async Task<string> ExecuteAsync()
    {
        return await Task.Factory.StartNew(() =>
        {
            if (!Directory.Exists(WorldSaveFolder))
                return null;

            string[] files = _filesPattern.SelectMany(pattern => Directory.EnumerateFiles(WorldSaveFolder, pattern)).ToArray();

            try
            {
                string tempZip = ClientInfo.GetRandomTempFile();
                Log.Debug("Creating temporary backup file at {0}", tempZip);

                CompressionProvider.Compress(tempZip, files);

                Log.Debug("Successfully compressed save files");

                return tempZip;
            }
            catch (UnauthorizedAccessException)
            {
                Log.Error("Failed to create backup for Monster Hunter: Rise. Try running HunterPie as administrator.");
                return null;
            }
            catch (Exception err)
            {
                Log.Error("Failed to create save files backup\n{0}", err);
                return null;
            }
        });
    }
}