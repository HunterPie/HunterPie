using HunterPie.Core.Assets;
using HunterPie.Core.Client;
using HunterPie.Core.Crypto;
using HunterPie.Core.Observability.Logging;
using HunterPie.Update.Gateway;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Update.Service;

internal class LocalizationUpdateService(
    UpdateGateway gateway,
    IAssetResolver resolver
)
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public async Task InvokeAsync()
    {
        Dictionary<string, string> latestChecksum = await gateway.GetLocalizationsChecksumAsync();

        foreach ((string name, string checksum) in latestChecksum)
        {

            string fileName = name.Replace("localization/", string.Empty);
            string localFilePath = ClientInfo.GetPathFor($"Languages/{fileName}");

            string localChecksum = File.Exists(localFilePath)
                ? await HashService.ChecksumAsync(localFilePath)
                : string.Empty;

            if (checksum == localChecksum)
                continue;

            _logger.Debug($"Downloading {name}... Remote checksum: {checksum} | Local checksum: {localChecksum}");

            try
            {
                string resolvedUri = await resolver.Resolve($"/{name}");

                File.Move(
                    sourceFileName: resolvedUri,
                    destFileName: localFilePath,
                    overwrite: true
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to download localization file: {ex}");
            }
        }
    }
}