using HunterPie.Core.Networking.Http;
using HunterPie.Core.Networking.Http.Events;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Localization;
using HunterPie.Integrations.Poogie.Localization.Models;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.Integrations.Poogie.Version.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Update.Remote;

public class UpdateApi
{
    private readonly PoogieVersionConnector _versionConnector = new();
    private readonly PoogieLocalizationConnector _localizationConnector = new();

    public async Task<string> GetLatestVersion()
    {
        PoogieResult<VersionResponse> response = await _versionConnector.Latest();

        return response.Response is not { } resp ? null : resp.LatestVersion;
    }

    public async Task<bool> DownloadVersion(string version, string output, EventHandler<DownloadEventArgs> callback)
    {
        using HttpClientResponse resp = await _versionConnector.Download(version);

        if (resp.StatusCode != HttpStatusCode.OK)
            return false;

        resp.OnDownloadProgressChanged += callback;
        await resp.DownloadAsync(output);
        resp.OnDownloadProgressChanged -= callback;

        return true;
    }

    public async Task<Dictionary<string, string>> GetLocalizationsChecksum()
    {
        PoogieResult<LocalizationResponse> result = await _localizationConnector.GetChecksums();

        return result.Response is not { } resp ? new() : resp.Localizations;
    }
}