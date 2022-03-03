using System;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using Newtonsoft.Json;
using PoogieClient = HunterPie.Core.Http.Poogie;

namespace HunterPie.Internal.Poogie
{
    public class RemoteCrashReporter
    {
        public const string REPORT_CRASH = "/v1/report/crash";

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
        }

        public static void Send(Exception exception)
        {
            CrashReportSchema schema = new()
            {
                Version = ClientInfo.Version.ToString(),
                GameBuild = "-",
                Exception = exception.GetType().ToString(),
                Stacktrace = exception.ToString()
            };

            PoogieClient poogie = PoogieFactory.Default()
                                               .Post(REPORT_CRASH)
                                               .WithJson(schema)
                                               .Build();

            poogie.Request();
        }
    }
}
