using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Report.Models;

public record CrashReportContextRequest(
    [JsonProperty("ram_total")] ulong RamTotal,
    [JsonProperty("ram_used")] long RamUsed,
    [JsonProperty("windows_version")] string WindowsVersion
);