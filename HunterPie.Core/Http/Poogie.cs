using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
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

        public List<string> Urls { get; set; } = new();
        public string Path { get; set; }
        public HttpMethod Method { get; set; }
        public HttpContent Content { get; set; }
        public TimeSpan Timeout { get; set; }
        public Dictionary<string, string> Headers { get; } = new();

        public async Task<PoogieResponse> RequestAsync()
        {
            foreach (string host in Urls)
            {
                _client = new() { Timeout = Timeout };
                _request = new(Method, $"{host}{Path}");

                foreach (var (key, value) in Headers)
                {
                    if (string.IsNullOrEmpty(value))
                        continue;

                    _request.Headers.Add(key, value);
                }

                HttpResponseMessage res;
                try
                {
                    res = await _client.SendAsync(_request);
                }
                catch (Exception err)
                {
                    Log.Debug($"Failed to request host {host}, trying next one...\n{err}");
                    _client.Dispose();
                    _request.Dispose();

                    continue;
                }

                PoogieResponse response = new(res);
                return response;
            }

            Log.Error("Could not reach any of HunterPie's HTTP hosts.");

            return new PoogieResponse(null);
        }

        public PoogieResponse Request()
        {
            foreach (string host in Urls)
            {
                _client = new() { Timeout = Timeout };
                _request = new(Method, $"{host}{Path}");

                if (Content is not null)
                    _request.Content = Content;

                foreach (var (key, value) in Headers)
                {
                    if (string.IsNullOrEmpty(value))
                        continue;

                    _request.Headers.Add(key, value);
                }

                HttpResponseMessage res;
                try
                {
                    res = _client.Send(_request);
                }
                catch (Exception err)
                {
                    Log.Debug($"Failed to request host {host}, trying next one...\n{err}");
                    _client.Dispose();
                    _request.Dispose();

                    continue;
                }

                PoogieResponse response = new(res);
                return response;
            }

            Log.Error("Could not reach any of HunterPie's HTTP hosts.");

            return new PoogieResponse(null);
        }

        public void Dispose()
        {
            _request?.Dispose();
            _client?.Dispose();
        }
    }
}
