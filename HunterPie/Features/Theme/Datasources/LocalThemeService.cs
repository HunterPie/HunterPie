using HunterPie.Core.Client;
using HunterPie.Core.Json;
using HunterPie.Features.Theme.Datasources.Model;
using HunterPie.Features.Theme.Entity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.Datasources;

internal class LocalThemeService
{
    public async Task<IReadOnlyCollection<LocalThemeManifest>> ListAll()
    {
        IEnumerable<string> manifestPaths = Directory.GetDirectories(ClientInfo.ThemesPath)
            .Select(it => Path.Join(ClientInfo.ThemesPath, it, "manifest.json"))
            .Where(File.Exists);

        var manifests = new List<LocalThemeManifest>();

        foreach (string manifestPath in manifestPaths)
        {
            string manifestStream = await File.ReadAllTextAsync(manifestPath);
            string? themeFolder = Path.GetDirectoryName(manifestPath);

            if (themeFolder is null)
                continue;

            ThemeManifest manifest = JsonProvider.Deserializer<ThemeManifestModel>(manifestStream)
                .ToEntity();

            manifests.Add(
                item: new LocalThemeManifest(
                    Path: themeFolder,
                    Manifest: manifest
                )
            );
        }

        return manifests;
    }
}