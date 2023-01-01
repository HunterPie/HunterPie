using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Settings.Models;

internal record ClientSettingsResponse(
    [JsonProperty("configuration")] string Configuration
);
