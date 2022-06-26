using HunterPie.Core.API.Schemas;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Core.API
{
#nullable enable
    public static class PoogieApi
    {
        const string VERSION_PATH = "/v1/version";
        const string CRASH_PATH = "/v1/report/crash";
        const string SESSION_PATH = "/v1/session";
        const string SUPPORTER_PATH = "/v1/supporter";

        const string SUPPORTER_HEADER = "X-Supporter-Token";

        const double DEFAULT_TIMEOUT = 10;
        static private readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);

        public static async Task<VersionResSchema?> GetLatestVersion()
        {
            using Poogie request = PoogieFactory.Default()
                .Get(VERSION_PATH)
                .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
                .WithTimeout(DefaultTimeout)
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
                .WithTimeout(DefaultTimeout)
                .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (!resp.Success)
                return null;

            if ((int)resp.Status >= 400)
                return null;

            SessionResSchema schema = await resp.AsJson<SessionResSchema>();
            return schema;
        }

        public static async Task<SessionResSchema> EndSession()
        {
            using Poogie request = PoogieFactory.Default()
                .Post(SESSION_PATH + "/end")
                .WithTimeout(DefaultTimeout)
                .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (!resp.Success)
                return null;

            if (resp.Status >= HttpStatusCode.BadRequest)
                return null;

            SessionResSchema schema = await resp.AsJson<SessionResSchema>();
            return schema;
        }

        public static async Task<SupporterValidationResSchema?> ValidateSupporterToken()
        {
            using Poogie request = PoogieFactory.Default()
                .Get(SUPPORTER_PATH + "/verify")
                .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
                .WithTimeout(DefaultTimeout)
                .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (!resp.Success)
                return null;

            if (resp.Status >= HttpStatusCode.BadRequest)
                return null;

            SupporterValidationResSchema schema = await resp.AsJson<SupporterValidationResSchema>();
            return schema;
        }
    }
#nullable restore
}
