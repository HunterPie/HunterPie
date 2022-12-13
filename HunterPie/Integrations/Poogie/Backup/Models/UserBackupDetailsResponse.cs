using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Backup.Models;
internal record UserBackupDetailsResponse(
    [JsonProperty("count")] int Count,
    [JsonProperty("max_count")] int MaxCount,
    [JsonProperty("backups")] BackupResponse[] Backups
);
