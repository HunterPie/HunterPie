using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class UserBackupDetailsResponse
{
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("max_count")]
    public int MaxCount { get; set; }

    [JsonProperty("backups")]
    public BackupResponse[] Backups { get; set; }
}
