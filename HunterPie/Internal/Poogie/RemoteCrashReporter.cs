using HunterPie.Core.Client;
using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace HunterPie.Internal.Poogie;

public class RemoteCrashReporter
{
    public const string REPORT_CRASH = "/v1/report/crash";

    private class CrashReportContextSchema
    {
        [JsonProperty("ram_total")]
        public ulong RamTotal { get; init; }

        [JsonProperty("ram_used")]
        public long RamUsed { get; init; }

        [JsonProperty("windows_version")]
        public string WindowsVersion { get; init; }
    }

    private class CrashReportSchema
    {
        [JsonProperty("version")]
        public string Version { get; init; }

        [JsonProperty("game_build")]
        public string GameBuild { get; init; }

        [JsonProperty("exception")]
        public string Exception { get; init; }

        [JsonProperty("stacktrace")]
        public string Stacktrace { get; init; }

        [JsonProperty("is_ui_error")]
        public bool IsUiError { get; init; }

        [JsonProperty("context")]
        public CrashReportContextSchema Context { get; init; }
    }

    public static void Send(Exception exception, bool isUIError = false)
    {
        ComputerInfo computerInfo = new();
        using var process = Process.GetCurrentProcess();

        CrashReportSchema schema = new()
        {
            Version = ClientInfo.Version.ToString(),
            GameBuild = "-",
            Exception = exception.GetType().ToString(),
            Stacktrace = exception.ToString(),
            IsUiError = isUIError,
            Context = new CrashReportContextSchema
            {
                RamTotal = computerInfo.TotalPhysicalMemory,
                RamUsed = process.PrivateMemorySize64,
                WindowsVersion = Environment.OSVersion.ToString(),
            }
        };

        HttpClient poogie = PoogieProvider.Default()
                                          .Post(REPORT_CRASH)
                                          .WithHeader("X-Ui-Error", isUIError.ToString())
                                          .WithJson(schema)
                                          .Build();

        using HttpClientResponse response = poogie.Request();
    }
}
