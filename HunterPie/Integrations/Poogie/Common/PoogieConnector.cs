using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Common;
internal class PoogieConnector
{
    public async Task<PoogieResult<T>> Get<T>(string path, Dictionary<string, object>? query = null)
    {
        HttpClientBuilder clientBuilder = PoogieProvider.Default()
            .Get(path);

        if (query is not null)
            clientBuilder.WithQuery(query);

        using HttpClient client = clientBuilder.Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<T>.From(response);
    }

    public async Task<HttpClientResponse> Download(string path)
    {
        using HttpClient client = PoogieProvider.Default()
            .Get(path)
            .WithTimeout(TimeSpan.FromSeconds(60))
            .Build();

        return await client.RequestAsync();
    }

    public async Task<PoogieResult<TOut>> Post<TIn, TOut>(string path, TIn payload)
    {
        using HttpClient client = PoogieProvider.Default()
            .Post(path)
            .WithJson(payload)
            .Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<TOut>.From(response);
    }

    public async Task<PoogieResult<T>> SendFile<T>(string path, string filename)
    {
        using HttpClient client = PoogieProvider.Default()
            .Post(path)
            .WithTimeout(TimeSpan.FromSeconds(60))
            .WithFile("file", filename)
            .Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<T>.From(response);
    }

    public async Task<PoogieResult<TOut>> Delete<TIn, TOut>(string path, TIn payload)
    {
        using HttpClient client = PoogieProvider.Default()
            .Delete(path)
            .WithJson(payload)
            .Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<TOut>.From(response);
    }

    public async Task<PoogieResult<TOut>> Patch<TIn, TOut>(string path, TIn payload)
    {
        using HttpClient client = PoogieProvider.Default()
            .Patch(path)
            .WithJson(payload)
            .Build();

        using HttpClientResponse response = await client.RequestAsync();

        return await PoogieResult<TOut>.From(response);
    }
}