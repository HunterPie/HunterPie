using HunterPie.Core.Client;
using HunterPie.Core.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Core.Remote;

public class CDN
{
    private const string CDN_BASE_URL = "https://cdn.hunterpie.com";

    private static readonly HashSet<string> _notFoundCache = new();

    public static async Task<string> GetMonsterIconUrl(string imagename)
    {
        if (_notFoundCache.Contains(imagename))
            return null;

        string localImage = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{imagename}.png");

        if (File.Exists(localImage))
            return localImage;

        using Poogie request = new PoogieBuilder(CDN_BASE_URL)
                                    .Get($"/Assets/Monsters/Icons/{imagename}.png")
                                    .WithTimeout(TimeSpan.FromSeconds(5))
                                    .Build();

        using PoogieResponse response = await request.RequestAsync();
        {
            if (!response.Success)
                return null;

            if (response.Status != HttpStatusCode.OK)
            {
                _ = _notFoundCache.Add(imagename);
                return null;
            }

            await response.Download(localImage);
        }

        return localImage;
    }

    public static async Task<string> GetAsset(string uri)
    {
        string fileName = Path.GetFileName(uri);
        string localImage = ClientInfo.GetPathFor($"Assets/Cache/{fileName}");

        if (File.Exists(localImage))
            return localImage;

        string filePath = uri.Replace(CDN_BASE_URL, string.Empty);

        using Poogie request = new PoogieBuilder(CDN_BASE_URL)
            .Get(filePath)
            .WithTimeout(TimeSpan.FromSeconds(5))
            .Build();

        using PoogieResponse response = await request.RequestAsync();

        if (!response.Success)
            return null;

        if (response.Status != HttpStatusCode.OK)
            return null;

        await response.Download(localImage);

        return localImage;
    }
}
