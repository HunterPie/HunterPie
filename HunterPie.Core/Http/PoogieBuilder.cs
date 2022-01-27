using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace HunterPie.Core.Http
{
    public class PoogieBuilder
    {
        public string Url { get; private set; }
        public HttpMethod Method { get; private set; }
        public HttpContent Content { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public PoogieBuilder Get(string url)
        {
            Url = url;
            Method = HttpMethod.Get;

            return this;
        }

        public PoogieBuilder Post(string url)
        {
            Url = url;
            Method = HttpMethod.Post;

            return this;
        }

        public PoogieBuilder WithJson<T>(T json)
        {
            string serialized = JsonConvert.SerializeObject(json);
            Content = new StringContent(serialized, Encoding.UTF8, "application/json");

            return this;
        }

        public PoogieBuilder WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public Poogie Build()
        {
            return new Poogie(this);
        }
    }
}
