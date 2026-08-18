using HunterPie.Core.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Core.Networking.Http.Models;

public record class Response
{
    private const int BufferSize = 4096;

    public record class Success(
        HttpStatusCode StatusCode,
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

        public async Task DownloadAsync(string outputPath)
        {
            try
            {
                FileStream file = File.Create(
                    path: outputPath,
                    bufferSize: BufferSize
                );

                await Body.BaseStream.CopyToAsync(
                    destination: file,
                    bufferSize: BufferSize
                );
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
