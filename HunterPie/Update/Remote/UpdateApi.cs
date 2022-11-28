using HunterPie.Core.API;
using HunterPie.Core.API.Entities;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using HunterPie.Core.Http.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Update.Remote;

public class UpdateApi
{
    public async Task<string> GetLatestVersion()
    {
        PoogieApiResult<VersionResponse> response = await PoogieApi.GetLatestVersion();

        if (response is null || !response.Success)
            return null;

        return response.Response?.LatestVersion;
    }

    public async Task<bool> DownloadVersion(string version, EventHandler<PoogieDownloadEventArgs> callback)
    {
        using PoogieResponse resp = await PoogieApi.DownloadVersion(version);

        if (!resp.Success)
            return false;

        if (resp.Status != HttpStatusCode.OK)
            return false;

        resp.OnDownloadProgressChanged += callback;
        await resp.Download(ClientInfo.GetPathFor(@"temp/HunterPie.zip"));
        resp.OnDownloadProgressChanged -= callback;

        return true;
    }

    public async Task<Dictionary<string, string>> GetLocalizationsChecksum()
    {
        PoogieApiResult<LocalizationsResponse> result = await PoogieApi.GetLocalizationsChecksums();

        if (result.Response is null)
            return new();

        return result.Response.Localizations;
    }
}
