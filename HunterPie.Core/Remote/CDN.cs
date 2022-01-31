using HunterPie.Core.Client;
using HunterPie.Core.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

            string localImage = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{imagename}.png");

            if (File.Exists(localImage))
                return localImage;


            using Poogie request = new PoogieBuilder(CDN_BASE_URL)
                                        .Get($"/Assets/Monsters/Icons/{imagename}.png")
                                        .WithTimeout(TimeSpan.FromSeconds(5))
                                        .Build();

            using PoogieResponse response = await request.RequestAsync();
            {
                if (!response.Success)
                    return null;

                if (response.Status != HttpStatusCode.OK)
                {
                    _notFoundCache.Add(imagename);
                    return null;
                }

                await response.Download(localImage);
            }

            return localImage;
        }
    }
}
