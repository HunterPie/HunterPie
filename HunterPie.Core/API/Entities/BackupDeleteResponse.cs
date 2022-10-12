using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class BackupDeleteResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }
}
