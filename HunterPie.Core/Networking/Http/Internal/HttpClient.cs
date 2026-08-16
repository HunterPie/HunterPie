using HunterPie.Core.Networking.Http.Exceptions;
using HunterPie.Core.Networking.Http.Intercept;
using HunterPie.Core.Networking.Http.Models;
using HunterPie.Core.Observability.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetHttpClient = System.Net.Http.HttpClient;

namespace HunterPie.Core.Networking.Http.Internal;

internal class HttpClient(
    NetHttpClient client,
    string[] urls,
    int retry,
    IReadOnlyList<IHttpInterceptor> interceptors
) : IHttpClient
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public Task<Response> DeleteAsync(string path, RequestConfiguration? cfg = null) =>
        RequestAsync(method: HttpMethod.Delete, path, cfg);

    public Task<Response> GetAsync(string path, RequestConfiguration? cfg = null) =>
        RequestAsync(method: HttpMethod.Get, path, cfg);

    public Task<Response> PatchAsync(string path, RequestConfiguration? cfg = null) =>
        RequestAsync(method: HttpMethod.Patch, path, cfg);

    public Task<Response> PostAsync(string path, RequestConfiguration? cfg = null) =>
        RequestAsync(method: HttpMethod.Post, path, cfg);

    public Task<Response> PutAsync(string path, RequestConfiguration? cfg = null) =>
        RequestAsync(method: HttpMethod.Put, path, cfg);

    private async Task<Response> RequestAsync(HttpMethod method, string path, RequestConfiguration? cfg)
    {
        foreach (string url in urls)
        {
            for (int attempt = 0; attempt < Math.Max(1, retry); attempt++)
            {
                string endpoint = url + path;
                Response response = await DoRequestAsync(
                    method: method,
                    url: endpoint,
                    cfg: cfg
                );

                if (response is Response.Success { StatusCode: < HttpStatusCode.InternalServerError })
                    return response;

                if (response is Response.Error err)
                    _logger.Warning($"Failed to complete request to {endpoint}: {err.Exception}");

                var backoff = TimeSpan.FromMilliseconds((attempt + 1) * 250);
                await Task.Delay(backoff);
            }
        }

        return new Response.Error(
            Exception: new NetworkException("all retry attempts failed")
        );
    }

    private async Task<Response> DoRequestAsync(HttpMethod method, string url, RequestConfiguration? cfg)
    {
        using var message = new HttpRequestMessage(method, url);
        var options = new HttpRequestOptions(message);

        if (cfg is { } configure)
            configure(options);

        var chain = new HttpChain(
            interceptors: interceptors,
            executeRequest: async (request) => await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
        );

        try
        {
            HttpResponseMessage response = await chain.NextAsync(message);

            return new Response.Success(
                StatusCode: response.StatusCode,
                Body: new StreamReader(await response.Content.ReadAsStreamAsync())
            );
        }
        catch (Exception ex)
        {
            return new Response.Error(ex);
        }
    }


}
