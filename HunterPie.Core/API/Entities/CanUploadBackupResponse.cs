using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HunterPie.Core.API.Entities;
public class CanUploadBackupResponse
{
    [JsonProperty("can_upload")]
    public bool CanUpload { get; set; }
}
