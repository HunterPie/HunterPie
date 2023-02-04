using HunterPie.Core.Client;
using HunterPie.Core.Crypto;
using HunterPie.Core.Logger;
using HunterPie.Core.Networking.Http.Events;
using HunterPie.Core.Remote;
using HunterPie.Update.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace HunterPie.Update;

internal class UpdateService
{
    private readonly UpdateApi _api = new();
    private string latest;

    public async Task<Version> GetLatestVersion()
    {
        latest = await _api.GetLatestVersion();

        if (latest is null)
            return null;

        var parsed = new Version(latest);

        return parsed;
    }

    public async Task<bool> DownloadZip(EventHandler<DownloadEventArgs> callback) => await _api.DownloadVersion(latest, callback);

    public bool ExtractZip()
    {
        string filePath = ClientInfo.GetPathFor(@"temp/HunterPie.zip");
        string extractPath = ClientInfo.GetPathFor(@"temp/HunterPie");

        try
        {
            ZipFile.ExtractToDirectory(filePath, extractPath);
        }
        catch (Exception err)
        {
            Log.Error(err.ToString());
            Directory.Delete(ClientInfo.GetPathFor("temp"), true);
            return false;
        }

        return true;
    }

    public async Task<Dictionary<string, string>> IndexAllFilesRecursively(string basePath, string relativePath = "", Dictionary<string, string> files = null)
    {
        files ??= new Dictionary<string, string>();

        foreach (string entry in Directory.GetFileSystemEntries(basePath))
        {
            FileAttributes attrib = File.GetAttributes(entry);
            string absolute = Path.Combine(basePath, entry);
            string relative = Path.Combine(relativePath, Path.GetRelativePath(basePath, entry));

            if ((attrib & FileAttributes.Directory) == FileAttributes.Directory)
                _ = await IndexAllFilesRecursively(absolute, relative, files);
            else
                files.Add(relative, await HashService.ChecksumAsync(absolute));

        }

        return files;
    }

    public bool ReplaceOldFiles(Dictionary<string, string> local, Dictionary<string, string> remote)
    {
        List<string> files = new();
        foreach ((string path, string hash) in remote)
        {
            if (!local.ContainsKey(path))
            {
                files.Add(path);
                continue;
            }

            string localHash = local[path];

            if (localHash != hash)
            {
                string oldFile = ClientInfo.GetPathFor(path) + ".old";

                if (File.Exists(oldFile))
                    File.Delete(oldFile);

                File.Move(ClientInfo.GetPathFor(path), oldFile);
                files.Add(path);
            }
        }

        foreach (string file in files)
        {
            string updatedFile = Path.Combine(ClientInfo.ClientPath, @"temp/HunterPie", file);
            string localFile = ClientInfo.GetPathFor(file);

            if (!File.Exists(localFile))
                _ = Directory.CreateDirectory(
                    Path.GetDirectoryName(localFile)
                );

            File.Move(updatedFile, localFile);
        }

        // Cleanup
        Directory.Delete(ClientInfo.GetPathFor("temp"), true);

        return true;
    }

    public void CleanupOldFiles()
    {
        Stack<string> directories = new();

        directories.Push(ClientInfo.ClientPath);

        while (directories.Count > 0)
            foreach (string entry in Directory.GetFileSystemEntries(directories.Pop()))
            {
                FileAttributes attrib = File.GetAttributes(entry);

                if (attrib.HasFlag(FileAttributes.Directory))
                    directories.Push(entry);
                else
                if (entry.EndsWith(".old"))
                    try
                    {
                        File.Delete(entry);
                    }
                    catch (Exception err)
                    {
                        Log.Error(err.ToString());
                    }
            }
    }

    public async Task UpdateLocalizationFiles()
    {
        Dictionary<string, string> remoteChecksums = await _api.GetLocalizationsChecksum();

        foreach ((string remoteName, string remoteChecksum) in remoteChecksums)
        {
            string fileName = remoteName.Replace("localization/", string.Empty);
            string localFilePath = ClientInfo.GetPathFor($"Languages/{fileName}");

            string localChecksum = File.Exists(localFilePath)
                ? await HashService.ChecksumAsync(localFilePath)
                : "";

            if (remoteChecksum == localChecksum)
                continue;

            Log.Debug("Downloading {0}... Remote checksum: {1} | Local checksum: {2}", remoteName, remoteChecksum, localChecksum);
            await CDN.GetFile($"/{remoteName}", localFilePath);
        }
    }
}
