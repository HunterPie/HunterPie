using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HunterPie.Core.Http
{
    public class PoogieBuilder
    {
        public List<string> Urls { get; private set; } = new();
        public string Path { get; private set; }
        public HttpMethod Method { get; private set; }
        public HttpContent Content { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public PoogieBuilder() { }
        public PoogieBuilder(string[] urls) { Urls.AddRange(urls); }
        public PoogieBuilder(string url) { Urls.Add(url); }

        public PoogieBuilder Get(string path)
        {
            Method = HttpMethod.Get;
            Path = path;

            return this;
        }

        public PoogieBuilder Post(string path)
        {
            Method = HttpMethod.Post;
            Path = path;

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
