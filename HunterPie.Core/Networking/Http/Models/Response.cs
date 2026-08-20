using HunterPie.Core.Json;
using HunterPie.Core.Networking.Http.Events;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Core.Networking.Http.Models;

public record class Response
{
    private const int BufferSize = 4096;

    public record class Success(
        HttpStatusCode StatusCode,
        HttpResponseMessage Message,
        StreamReader Body
    ) : Response, IDisposable
    {
        public async Task<T> JsonAsync<T>() where T : class
        {
            try
            {
                string body = await Body.ReadToEndAsync();
                return JsonProvider.Deserializer<T>(body);
            }
            finally
            {
                Body.Dispose();
            }
        }

        public async Task<string> TextAsync()
        {
            try
            {
                return await Body.ReadToEndAsync();
            }
            finally
            {
                Body.Dispose();
            }
        }

        public async Task DownloadAsync(string outputPath, DownloadEventHandler? callback = null)
        {
            long? totalBytes = Message.Content.Headers.ContentLength;

            long totalBytesRead = 0;
            Memory<byte> buffer = new byte[8192];

            try
            {
                string? directoryPath = Path.GetDirectoryName(outputPath);
                if (directoryPath is not null && !Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                await using var file = new FileStream(
                    path: outputPath,
                    mode: FileMode.Create,
                    access: FileAccess.Write,
                    share: FileShare.None,
                    bufferSize: buffer.Length,
                    useAsync: true
                );

                do
                {
                    int bytesRead = await Body.BaseStream.ReadAsync(buffer);
                    totalBytesRead += bytesRead;

                    var eventModel = new DownloadEvent(
                        IsLengthUnknown: totalBytes is null,
                        DownloadedBytes: totalBytesRead,
                        TotalBytes: totalBytes ?? 0
                    );

                    callback?.Invoke(eventModel);

                    if (bytesRead is 0)
                        return;

                    await file.WriteAsync(buffer[..bytesRead]);
                } while (true);
            }
            finally
            {
                Body.Dispose();
            }
        }

        public void Dispose()
        {
            Body.Dispose();
        }
    }

    public record class Error(
        Exception Exception
    ) : Response;
}
