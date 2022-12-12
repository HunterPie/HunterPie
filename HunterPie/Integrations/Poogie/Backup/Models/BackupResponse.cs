using Newtonsoft.Json;
using System;

namespace HunterPie.Integrations.Poogie.Backup.Models;

internal record BackupResponse(
    [JsonProperty("id")] string Id,
    [JsonProperty("size")] long Size,
    [JsonProperty("game_name")] string GameName,
    [JsonProperty("game_icon")] string GameIcon,
    [JsonProperty("uploaded_at")] DateTime UploadedAt
);
