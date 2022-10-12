using HunterPie.Core.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;

namespace HunterPie.Core.Http;

public class PoogieBuilder
{
    private readonly Poogie poogie = new();

    public PoogieBuilder() { }
    public PoogieBuilder(string[] urls) { poogie.Urls.AddRange(urls); }
    public PoogieBuilder(string url) { poogie.Urls.Add(url); }

    public PoogieBuilder Get(string path)
    {
        poogie.Method = HttpMethod.Get;
        poogie.Path = path;

        return this;
    }

    public PoogieBuilder WithHeader(string key, string value)
    {
        Debug.Assert(!string.IsNullOrEmpty(key));

        poogie.Headers.Add(key, value);

        return this;
    }

    public PoogieBuilder Post(string path)
    {
        Debug.Assert(path.StartsWith("/"));

        poogie.Method = HttpMethod.Post;
        poogie.Path = path;

        return this;
    }

    public PoogieBuilder Delete(string path)
    {
        Debug.Assert(path.StartsWith("/"));

        poogie.Method = HttpMethod.Delete;
        poogie.Path = path;

        return this;
    }

    public PoogieBuilder WithJson<T>(T json)
    {
        string serialized = JsonProvider.Serializer(json);
        poogie.Content = new StringContent(serialized, Encoding.UTF8, "application/json");

        return this;
    }

    public PoogieBuilder WithFile(string name, string path)
    {
        Stream fileStream = File.OpenRead(path);
        var content = new StreamContent(fileStream);
        var form = new MultipartFormDataContent()
        {
            Headers =
            {
                ContentType =
                {
                    MediaType = "multipart/form-data"
                }
            }
        };
        form.Add(content, name, name);
        poogie.Content = form;

        return this;
    }

    public PoogieBuilder WithTimeout(TimeSpan timeout)
    {
        poogie.Timeout = timeout;
        return this;
    }

    public PoogieBuilder WithRetry(int retryCount)
    {
        poogie.Retry = retryCount;
        return this;
    }
    public Poogie Build() => poogie;
}
