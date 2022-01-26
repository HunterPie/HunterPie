using HunterPie.Core.Client;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Core.Remote
{
    public class CDN
    {
        const string CDN_BASE_URL = "https://cdn.hunterpie.com";

        private static HashSet<string> _notFoundCache = new();

        public static string GetMonsterIcon(string imageName)
        {
            return "";
        }

        public static async Task<string> GetMonsterIconUrl(string imagename)
        {
            if (_notFoundCache.Contains(imagename))
                return null;

            string url = $"{CDN_BASE_URL}/Assets/Monsters/Icons/{imagename}.png";
            string localImage = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{imagename}.png");

            if (File.Exists(localImage))
                return localImage;

            using (HttpClient client = new())
            {
                using (HttpRequestMessage req = new(HttpMethod.Get, url))
                {
                    using (HttpResponseMessage response = await client.SendAsync(req))
                    {
                        if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            _notFoundCache.Add(imagename);
                            return null;
                        }

                        byte[] data = await response.Content.ReadAsByteArrayAsync();

                        await File.WriteAllBytesAsync(
                            localImage,
                            data
                        );

                        return localImage;
                    }
                }
            }

        }
    }
}
