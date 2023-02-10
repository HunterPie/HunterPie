using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Backup.Models;

internal record CanUploadBackupResponse([JsonProperty("can_upload")] bool CanUpload);
