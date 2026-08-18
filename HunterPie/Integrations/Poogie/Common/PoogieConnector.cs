using HunterPie.Core.Networking.Http;
using HunterPie.Core.Networking.Http.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Common;

internal class PoogieConnector(IHttpClient client) : IPoogieClientAsync
{
    public async Task<PoogieResult<T>> GetAsync<T>(string path, Dictionary<string, object>? query = null)
    {
        Response response = await client.GetAsync(path, (cfg) =>
        {
            if (query is null)
                return;

            foreach ((string key, object? value) in query)
                cfg.WithQuery(key, value);
        });

        return response switch
        {
            Response.Error err => throw err.Exception,
            Response.Success res => await PoogieResult<T>.FromAsync(res),
            _ => throw new UnreachableException()
        };
    }

    public async Task<Response.Success> DownloadAsync(string path)
    {
        Response response = await client.GetAsync(path);

        return response switch
        {
            Response.Error err => throw err.Exception,
            Response.Success res => res,
            _ => throw new UnreachableException()
        };
    }

    public async Task<PoogieResult<TOut>> PostAsync<TIn, TOut>(string path, TIn payload)
    {
        Response response = await client.PostAsync(
            path: path,
            (cfg) =>
                cfg
                .WithJson(payload)
        );

        return response switch
        {
            Response.Error err => throw err.Exception,
            Response.Success res => await PoogieResult<TOut>.FromAsync(res),
            _ => throw new UnreachableException()
        };
    }

    public async Task<PoogieResult<T>> SendFileAsync<T>(string path, string filename)
    {
        Response response = await client.PostAsync(
            path: path,
            (cfg) =>
                cfg
                .WithFile("file", filename)
                .WithTimeout(TimeSpan.FromSeconds(60))
        );

        return response switch
        {
            Response.Error err => throw err.Exception,
            Response.Success res => await PoogieResult<T>.FromAsync(res),
            _ => throw new UnreachableException()
        };
    }

    public async Task<PoogieResult<TOut>> DeleteAsync<TIn, TOut>(string path, TIn payload)
    {
        Response response = await client.DeleteAsync(
            path: path,
            (cfg) =>
                cfg
                .WithJson(payload)
        );

        return response switch
        {
            Response.Error err => throw err.Exception,
            Response.Success res => await PoogieResult<TOut>.FromAsync(res),
            _ => throw new UnreachableException()
        };
    }

    public async Task<PoogieResult<TOut>> PatchAsync<TIn, TOut>(string path, TIn payload)
    {
        Response response = await client.PatchAsync(
            path: path,
            (cfg) =>
                cfg
                .WithJson(payload)
        );

        return response switch
        {
            Response.Error err => throw err.Exception,
            Response.Success res => await PoogieResult<TOut>.FromAsync(res),
            _ => throw new UnreachableException()
        };
    }
}