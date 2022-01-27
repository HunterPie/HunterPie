using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Core.Http
{
    /// <summary>
    /// HunterPie's HTTP client with native support for the Poogie API
    /// </summary>
    public class Poogie : IDisposable
    {
        private HttpClient _client;
        private HttpRequestMessage _request;

        public string Url { get; }
        public HttpMethod Method { get; }
        public HttpContent Content { get; }
        public TimeSpan Timeout { get; }

        public Poogie(PoogieBuilder builder)
        {
            Url = builder.Url;
            Method = builder.Method;
            Content = builder.Content;
            Timeout = builder.Timeout;
        }

        public async Task<PoogieResponse> RequestAsync()
        {
            _client = new() { Timeout = Timeout };
            _request = new(Method, Url);
            HttpResponseMessage res = await _client.SendAsync(_request);

            PoogieResponse response = new(res);

            return response;
        }

        public void Dispose()
        {
            _request.Dispose();
            _client.Dispose();
        }
    }
}
