using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Report.Models;

public record CrashReportRequest(
    [JsonProperty("version")] string Version,
    [JsonProperty("game_build")] string GameBuild,
    [JsonProperty("exception")] string Exception,
    [JsonProperty("stacktrace")] string Stacktrace,
    [JsonProperty("is_ui_error")] bool IsUiError,
    [JsonProperty("context")] CrashReportContextRequest Context
);