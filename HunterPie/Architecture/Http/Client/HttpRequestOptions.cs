using HunterPie.Core.Json;
using HunterPie.Core.Networking.Http;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;

namespace HunterPie.Architecture.Http.Client;

internal class HttpRequestOptions(HttpRequestMessage message) : IRequestOptions
{
    public CancellationTokenSource? TokenSource { get; private set; }

    public IRequestOptions WithHeader(string key, string value)
    {
        message.Headers.Add(key, value);
        return this;
    }

    public IRequestOptions WithQuery(string key, object? value)
    {
        Uri? url = message.RequestUri;

        if (url is null || value is null)
            return this;

        var builder = new UriBuilder(url);

        NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);
        query.Add(key, value.ToString());

        message.RequestUri = builder.Uri;

        return this;
    }

    public IRequestOptions WithFile(string name, string path)
    {
        Stream stream = File.OpenRead(path);
        var content = new StreamContent(stream);
        var form = new MultipartFormDataContent
        {
            Headers =
            {
                ContentType = new("multipart/form-data")
            }
        };


        form.Add(content, name, name);

        message.Content = form;

        return this;
    }

    public IRequestOptions WithJson<T>(T obj)
    {
        string serialized = JsonProvider.Serializer(obj);

        message.Content = new StringContent(serialized, Encoding.UTF8, "application/json");

        return this;
    }

    public IRequestOptions WithTimeout(TimeSpan timeout)
    {
        TokenSource = new CancellationTokenSource(timeout);

        return this;
    }
}
