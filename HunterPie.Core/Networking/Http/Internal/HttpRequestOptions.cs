using HunterPie.Core.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;

namespace HunterPie.Core.Networking.Http.Internal;

internal class HttpRequestOptions(HttpRequestMessage message) : IRequestOptions
{
    public void WithHeader(string key, string value)
    {
        message.Headers.Add(key, value);
    }

    public void WithQuery(string key, object? value)
    {
        Uri? url = message.RequestUri;

        if (url is null || value is null)
            return;

        var builder = new UriBuilder(url);

        NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);
        query.Add(key, value.ToString());

        message.RequestUri = builder.Uri;
    }

    public void WithFile(string name, string path)
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
    }

    public void WithJSON<T>(T obj) where T : class
    {
        string serialized = JsonProvider.Serializer(obj);

        message.Content = new StringContent(serialized, Encoding.UTF8, "application/json; charset=utf-8");
    }
}
