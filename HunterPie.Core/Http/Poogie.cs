using HunterPie.Core.Logger;
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

        public string[] Urls { get; }
        public string Path { get; }
        public HttpMethod Method { get; }
        public HttpContent Content { get; }
        public TimeSpan Timeout { get; }

        public Poogie(PoogieBuilder builder)
        {
            Urls = builder.Urls.ToArray();
            Path = builder.Path;
            Method = builder.Method;
            Content = builder.Content;
            Timeout = builder.Timeout;
        }

        public async Task<PoogieResponse> RequestAsync()
        {
            foreach (string host in Urls)
            {
                _client = new() { Timeout = Timeout };
                _request = new(Method, $"{host}{Path}");

                HttpResponseMessage res;
                try
                {
                    res = await _client.SendAsync(_request);
                }
                catch
                {
                    Log.Debug($"Failed to request host {host}, trying next one...");
                    _client.Dispose();
                    _request.Dispose();

                    continue;
                }

                PoogieResponse response = new(res);
                return response;
            }

            return new PoogieResponse(null);
        }

        public void Dispose()
        {
            _request.Dispose();
            _client.Dispose();
        }
    }
}
