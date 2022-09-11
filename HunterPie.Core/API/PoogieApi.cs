using HunterPie.Core.API.Entities;
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
        const string SUPPORTER_PATH = "/v1/supporter";
        const string NOTIFICATIONS = "/v1/notifications";

        const string SUPPORTER_HEADER = "X-Supporter-Token";

        const double DEFAULT_TIMEOUT = 10;
        static private readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);

        private static async Task<T?> Get<T>(string path) where T : class
        {
            using Poogie request = PoogieFactory.Default()
                .Get(path)
                .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
                .WithTimeout(DefaultTimeout)
                .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (!resp.Success)
                return null;

            if (resp.Status >= HttpStatusCode.BadRequest)
                return null;

            return await resp.AsJson<T>();
        }

        public static async Task<VersionResponse?> GetLatestVersion()
        {
            VersionResponse? resp = await Get<VersionResponse>(VERSION_PATH);
            return resp;
        }

        public static async Task<SupporterValidationResponse?> ValidateSupporterToken()
        {
            SupporterValidationResponse? resp = await Get<SupporterValidationResponse>(SUPPORTER_PATH + "/verify");
            return resp;
        }

        public static async Task<Notification[]> GetNotifications()
        {
            Notification[]? resp = await Get<Notification[]>(NOTIFICATIONS);
            return resp ?? Array.Empty<Notification>();
        }
    }
#nullable restore
}
