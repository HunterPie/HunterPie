using System.Threading.Tasks;
using HunterPie.Core.Client;
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
            string url = "/v1/version";
            
            try
            {
                var res = await HttpClient.AsyncRequest(url);

                if (!res.Success)
                    return null;

                VersionSchema json = await res.Json<VersionSchema>();

                return json.LatestVersion;
            } catch { }

            return null;
        }

        public async Task DownloadVersion(string version)
        {
            string url = $"/v1/version/{version}";

            await HttpClient.AsyncRequest(url);

            await HttpClient.SaveAsFile(ClientInfo.GetPathFor(@"temp/HunterPie.zip"));
        }
    }
}
