using HunterPie.Core.Client;
using HunterPie.Core.Json;
using HunterPie.Features.Theme.Entity;
using HunterPie.Features.Theme.Repository.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.Repository;

internal class LocalThemeRepository
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Dictionary<string, LocalThemeManifest> _manifests = new();

    public async Task<IReadOnlyCollection<LocalThemeManifest>> ListAllAsync()
    {
        try
        {
            await _semaphore.WaitAsync();

            _manifests.Clear();

            IEnumerable<string> manifestPaths = Directory.GetDirectories(ClientInfo.ThemesPath)
                .Select(it => Path.Join(it, "theme.manifest.json"))
                .Where(File.Exists);

            foreach (string manifestPath in manifestPaths)
            {
                string manifestStream = await File.ReadAllTextAsync(manifestPath);
                DirectoryInfo? themeFolder = Directory.GetParent(manifestPath);

                if (themeFolder is null)
                    continue;

                ThemeManifest manifest = JsonProvider.Deserializer<ThemeManifestModel>(manifestStream)
                    .ToEntity();

                _manifests.Add(
                    key: manifest.Id,
                    value: new LocalThemeManifest(
                        Path: themeFolder.FullName,
                        Manifest: manifest
                    )
                );
            }

            return _manifests.Values;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public LocalThemeManifest? FindBy(string id)
    {
        try
        {
            _semaphore.Wait();
            return _manifests.GetValueOrDefault(id);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}