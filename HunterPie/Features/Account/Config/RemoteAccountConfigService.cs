using HunterPie.Core.Base64;
using HunterPie.Core.Client;
using HunterPie.Core.Json;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Settings;
using HunterPie.Integrations.Poogie.Settings.Models;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Config;

#nullable enable
internal class RemoteAccountConfigService
{

    private readonly PoogieClientSettingsConnector _settingsConnector = new();

    public async Task UploadClientConfig()
    {
        IAbstractHunterPieConfig config = ClientConfig.Config;
        string serializedConfig = JsonProvider.Serializer(config);
        string encodedConfig = Base64Service.Encode(serializedConfig);

        await _settingsConnector.UploadClientSettings(new ClientSettingsRequest(encodedConfig));
    }

    public async Task FetchClientConfig()
    {
        PoogieResult<ClientSettingsResponse> result = await _settingsConnector.GetClientSettings();

        if (result.Response is not { } response)
            return;

        if (result.Error is { Code: PoogieErrorCode.NOT_FOUND })
            return;

        string decodedConfig = Base64Service.Decode(response.Configuration);
        object config = JsonProvider.Deserializer(decodedConfig, ClientConfig.Config.GetType());

        ConfigHelper.WriteObject(
            ConfigHelper.GetFullPath(ClientConfig.CONFIG_NAME),
            config
        );
    }
}
