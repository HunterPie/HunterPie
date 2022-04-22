using System;
using System.Net;
using System.Threading.Tasks;
using HunterPie.Core.API;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using HunterPie.Core.Http.Events;

namespace HunterPie.Update.Remote
{ 
    public class UpdateApi
    {
        public async Task<string> GetLatestVersion()
        {
            var response = await PoogieApi.GetLatestVersion();
            return response?.LatestVersion;
        }

        public async Task DownloadVersion(string version, EventHandler<PoogieDownloadEventArgs> callback)
        {

            using Poogie request = PoogieFactory.Default()
                                    .Get($"/v1/version/{version}")
                                    .WithHeader("X-Supporter-Token", ClientConfig.Config.Client.SupporterSecretToken)
                                    .WithTimeout(TimeSpan.FromSeconds(10))
                                    .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (!resp.Success)
                return;

            if (resp.Status != HttpStatusCode.OK)
                return;

            resp.OnDownloadProgressChanged += callback;
            await resp.Download(ClientInfo.GetPathFor(@"temp/HunterPie.zip"));
            resp.OnDownloadProgressChanged -= callback;
        }
    }
}
