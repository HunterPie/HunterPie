using HunterPie.Core.API.Schemas;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using System;
using System.Threading.Tasks;

namespace HunterPie.Core.API
{
#nullable enable
    public static class PoogieApi
    {
        const string VERSION_PATH = "/v1/version";
        const string CRASH_PATH = "/v1/report/crash";
        const string SESSION_PATH = "/v1/session";

        const string SUPPORTER_HEADER = "X-Supporter-Token";

        const double DEFAULT_TIMEOUT = 10;

        public static async Task<VersionResSchema?> GetLatestVersion()
        {
            using Poogie request = PoogieFactory.Default()
                .Get(VERSION_PATH)
                .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
                .WithTimeout(TimeSpan.FromSeconds(DEFAULT_TIMEOUT))
                .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (!resp.Success)
                return null;

            if ((int)resp.Status >= 400)
                return null;

            VersionResSchema schema = await resp.AsJson<VersionResSchema>();
            return schema;
        }

        public static async Task<SessionResSchema?> GetSession()
        {
            using Poogie request = PoogieFactory.Default()
                .Get(SESSION_PATH)
                .WithTimeout(TimeSpan.FromSeconds(DEFAULT_TIMEOUT))
                .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (!resp.Success)
                return null;

            if ((int)resp.Status >= 400)
                return null;

            SessionResSchema schema = await resp.AsJson<SessionResSchema>();
            return schema;
        }
    }
#nullable restore
}
