using HunterPie.Core.Assets;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Game.Assets;
using HunterPie.Core.Networking.Http;
using HunterPie.Core.Networking.Http.Models;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Architecture.Images;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Integrations.CDN.Services;

internal class ContentDeliveryNetworkService(
    IHttpClient client
) : IAssetResolver, IMonsterIconResolver, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly HashSet<string> _notFoundCache = new();

    public async Task<string> Resolve(string path)
    {
        string filename = Path.GetFileName(path);
        string localFilePath = ClientInfo.GetPathFor(
            relative: Path.Join("Assets", "Cache", filename)
        );

        if (File.Exists(localFilePath))
            return localFilePath;

        Response response = await client.GetAsync(
            path: path,
            cfg => cfg.WithTimeout(TimeSpan.FromSeconds(5))
        );

        if (response is Response.Error err)
        {
            _logger.Error($"Failed to resolve asset: {err.Exception}");
            throw err.Exception;
        }

        if (response is not Response.Success res)
            throw new UnreachableException();

        if (res.StatusCode > HttpStatusCode.BadRequest)
            throw new Exception($"CDN returned status code {res.StatusCode}");

        await _semaphore.WaitAsync();
        try
        {
            await res.DownloadAsync(localFilePath);
        }
        finally
        {
            _semaphore.Release();
        }

        return localFilePath;
    }

    async Task<string?> IMonsterIconResolver.Get(
        GameType game,
        int monsterId,
        bool isQurio
    )
    {
        try
        {
            string monsterEm = $"{game}_{monsterId:00}";

            await _semaphore.WaitAsync();

            if (_notFoundCache.Contains(monsterEm))
                return null;

            string monsterIconPath = $"Assets/Monsters/Icons/{monsterEm}.png";

            string localFilePath = ClientInfo.GetPathFor(
                relative: monsterIconPath
            );

            if (!Path.Exists(localFilePath))
                return localFilePath;

            Response response = await client.GetAsync(
                path: monsterIconPath,
                cfg => cfg.WithTimeout(TimeSpan.FromSeconds(5))
            );

            if (response is Response.Error err)
            {
                _logger.Error($"Failed to resolve monster icon: {err.Exception}");
                throw err.Exception;
            }

            if (response is not Response.Success res)
                throw new UnreachableException();

            if (res.StatusCode > HttpStatusCode.BadRequest)
            {
                _notFoundCache.Add(monsterEm);
                return null;
            }

            await res.DownloadAsync(localFilePath);

            return isQurio switch
            {
                true => await ImageMergerService.MergeAsync(
                    outputPath: ClientInfo.GetPathFor($"Assets/Monsters/Icons/{monsterEm}-qurio.png"),
                    image: localFilePath,
                    mask: ClientInfo.GetPathFor("Assets/Monsters/Masks/qurio_mask.png")
                ),
                _ => localFilePath
            };
        }
        finally
        {
            _semaphore.Release();
        }
    }


    public void Dispose()
    {
        _semaphore.Dispose();
    }
}
