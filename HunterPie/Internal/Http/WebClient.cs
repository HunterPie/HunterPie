using System;
using System.Net;

namespace HunterPie.Internal.Http
{
    internal class WebClient : System.Net.WebClient
    {
        public int TimeOut { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest lWebRequest = base.GetWebRequest(address);
            lWebRequest.Timeout = TimeOut;
            ((HttpWebRequest)lWebRequest).CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
            ((HttpWebRequest)lWebRequest).ReadWriteTimeout = TimeOut;
            return lWebRequest;
        }
    }
}
