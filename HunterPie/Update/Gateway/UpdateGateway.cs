using HunterPie.Core.Networking.Http;
using HunterPie.Core.Networking.Http.Events;
using HunterPie.Core.Networking.Http.Exceptions;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Localization;
using HunterPie.Integrations.Poogie.Localization.Models;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.Integrations.Poogie.Version.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Update.Gateway;

internal class UpdateGateway(
    PoogieVersionConnector versionConnector,
    PoogieLocalizationConnector localizationConnector)
{
    private readonly PoogieVersionConnector _versionConnector = versionConnector;
    private readonly PoogieLocalizationConnector _localizationConnector = localizationConnector;

    public async Task<string?> GetLatestVersionAsync()
    {
        PoogieResult<VersionResponse> response = await _versionConnector.Latest();

        return response.Response is not { } resp ? null : resp.LatestVersion;
    }

    public async Task<bool> DownloadVersionAsync(string version, string output, EventHandler<DownloadEventArgs> callback)
    {
        using HttpClientResponse resp = await _versionConnector.Download(version);

        if (resp.StatusCode != HttpStatusCode.OK)
            throw new NetworkException($"Failed to request package file, was expecting status code 200 but got {resp.StatusCode}");

        resp.OnDownloadProgressChanged += callback;
        await resp.DownloadAsync(output);
        resp.OnDownloadProgressChanged -= callback;

        return true;
    }

    public async Task<Dictionary<string, string>> GetLocalizationsChecksumAsync()
    {
        PoogieResult<LocalizationResponse> result = await _localizationConnector.GetChecksumsAsync();

        return result.Response is not { } resp ? new() : resp.Localizations;
    }
}