using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Settings.Models;

internal record ClientSettingsRequest(
    [JsonProperty("configuration")] string Configuration
);