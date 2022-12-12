using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using HunterPie.Core.Zip;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Backups.Games;
internal class MHRBackupService : IBackupService
{
    private readonly string _savesPath;
    private const string SaveFilePattern = "*.bin";
    private readonly byte[] _magicBytes = { 0x44, 0x53, 0x53, 0x53 };
    private string RiseSaveFolder => _savesPath + "/1446780/remote/win64_save";

    public GameType Type => GameType.MHR;

    public MHRBackupService(string savesPath)
    {
        _savesPath = savesPath;
    }

    public async Task<string> ExecuteAsync()
    {
        return await Task.Factory.StartNew(() =>
        {
            if (!Directory.Exists(RiseSaveFolder))
                return null;

            string[] files = Directory.EnumerateFiles(RiseSaveFolder, SaveFilePattern)
                                      .ToArray();

            try
            {
                files = ValidateSaveFiles(files);

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

    private string[] ValidateSaveFiles(string[] files)
    {
        return files.Where(file =>
        {
            using FileStream fs = File.OpenRead(file);

            foreach (byte magicByte in _magicBytes)
                if ((byte)fs.ReadByte() != magicByte)
                    return false;

            return true;
        }).ToArray();
    }
}
