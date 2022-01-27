using System;
using System.Net;
using System.Threading.Tasks;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using HunterPie.Core.Http.Events;
using HunterPie.Internal.Http;
using Newtonsoft.Json;

namespace HunterPie.Update.Remote
{ 
    public class UpdateApi
    {
        private class VersionSchema
        {
            [JsonProperty("latest_version")]
            public string LatestVersion { get; set; }
        }

        const string BASE_URL = "https://api.hunterpie.com";
        public readonly AsyncHttpRequest HttpClient = new(BASE_URL);

        public async Task<string> GetLatestVersion()
        {
            string url = "https://api.hunterpie.com/v1/version";
            
            using Poogie request = new PoogieBuilder()
                                    .Get(url)
                                    .WithTimeout(TimeSpan.FromSeconds(10))
                                    .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (resp.Status != HttpStatusCode.OK)
                return null;

            VersionSchema schema = await resp.AsJson<VersionSchema>();

            return schema.LatestVersion;
        }

        public async Task DownloadVersion(string version, EventHandler<PoogieDownloadEventArgs> callback)
        {
            string url = $"https://api.hunterpie.com/v1/version/{version}";

            using Poogie request = new PoogieBuilder()
                                    .Get(url)
                                    .WithTimeout(TimeSpan.FromSeconds(10))
                                    .Build();

            using PoogieResponse resp = await request.RequestAsync();

            if (resp.Status != HttpStatusCode.OK)
                return;

            resp.OnDownloadProgressChanged += callback;
            await resp.Download(ClientInfo.GetPathFor(@"temp/HunterPie.zip"));
            resp.OnDownloadProgressChanged -= callback;
        }
    }
}
