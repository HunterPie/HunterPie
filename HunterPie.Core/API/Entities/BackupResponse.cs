using Newtonsoft.Json;
using System;

namespace HunterPie.Core.API.Entities;
public class BackupResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("size")]
    public long Size { get; set; }

    [JsonProperty("game_name")]
    public string GameName { get; set; }

    [JsonProperty("game_icon")]
    public string GameIcon { get; set; }

    [JsonProperty("uploaded_at")]
    public DateTime UploadedAt { get; set; }
}
