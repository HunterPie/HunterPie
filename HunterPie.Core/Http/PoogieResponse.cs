using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Http.Events;
using HunterPie.Core.Json;
using HunterPie.Core.Logger;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Core.Http
{
    public class PoogieResponse : IEventDispatcher, IDisposable
    {
        private HttpResponseMessage _response;
        public HttpStatusCode Status { get; }
        public HttpContent Content { get; }
        public bool Success { get; }
        public string Url { get; }

        public event EventHandler<PoogieDownloadEventArgs> OnDownloadProgressChanged;

        public PoogieResponse(HttpResponseMessage message)
        {
            if (message is null)
            {
                Success = false;
                return;
            }

            _response = message;
            Status = message.StatusCode;
            Content = message.Content;
            Success = true;
            Url = message.RequestMessage.RequestUri.AbsoluteUri;
        }

        public async Task<T> AsJson<T>()
        {
            string content = await Content.ReadAsStringAsync();

            return JsonProvider.Deserialize<T>(content);
        }

        public async Task<byte[]> AsRaw()
        {
            return await Content.ReadAsByteArrayAsync();
        }

        public async Task Download(string path)
        {
            long totalBytes = (long)Content.Headers.ContentLength;

            using Stream stream = await Content.ReadAsStreamAsync();
            long totalBytesRead = 0;
            bool isMoreToRead = true;
            byte[] buffer = new byte[8192];

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                using FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, true);

                do
                {
                    int bytesRead = await stream.ReadAsync(buffer);

                    if (bytesRead == 0)
                    {
                        isMoreToRead = false;
                        this.Dispatch(OnDownloadProgressChanged, new(totalBytesRead, totalBytes));
                        continue;
                    }

                    await output.WriteAsync(buffer.AsMemory(0, bytesRead));

                    totalBytesRead += bytesRead;

                    this.Dispatch(OnDownloadProgressChanged, new(totalBytesRead, totalBytes));

                } while (isMoreToRead);

            } catch(Exception err)
            {
                Log.Error(err.ToString());
                return;
            }
        }

        public void Dispose()
        {
            _response?.Dispose();
        }
    }
}
