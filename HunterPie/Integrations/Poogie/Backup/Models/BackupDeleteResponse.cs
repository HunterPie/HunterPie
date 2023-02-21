using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Backup.Models;

internal record BackupDeleteResponse([JsonProperty("id")] string Id);