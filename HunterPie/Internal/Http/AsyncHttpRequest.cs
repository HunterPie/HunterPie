using System;
using System.Threading.Tasks;
using System.Net.Cache;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace HunterPie.Internal.Http
{
    public class AsyncHttpRequest
    {
        private string _baseUrl;
        private byte[] _data;
        
        public bool Success { get; private set; }
        

        public EventHandler<DownloadProgressChangedEventArgs> OnDownloadProgressChanged;

        public AsyncHttpRequest(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<AsyncHttpRequest> AsyncRequest(string path)
        {
            Uri url = new Uri(_baseUrl + path);

            using (WebClient client = new WebClient() { TimeOut = 10000 })
            {
                client.Headers.Add(
                    HttpRequestHeader.CacheControl,
                    "no-store"
                );
                client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                client.DownloadProgressChanged += (src, args) => OnDownloadProgressChanged?.Invoke(this, args);

                try
                {
                    _data = await client.DownloadDataTaskAsync(url);
                    Success = true;
                } catch
                {
                    Success = false;
                    return this;
                }
            }

            return this;
        }

        public async Task<T> Json<T>()
        {
            string serialized = Encoding.UTF8.GetString(_data);

            return await Task.FromResult(JsonConvert.DeserializeObject<T>(serialized));
        }

        public async Task SaveAsFile(string filename)
        {
            if (!Directory.Exists(filename))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));

            using (FileStream stream = File.OpenWrite(filename))
            {
                await stream.WriteAsync(_data);
            }
        }
    }
}
