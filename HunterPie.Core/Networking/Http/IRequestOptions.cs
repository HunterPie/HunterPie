using System;

namespace HunterPie.Core.Networking.Http;

public interface IRequestOptions
{
    IRequestOptions WithHeader(string key, string value);

    IRequestOptions WithQuery(string key, object? value);

    IRequestOptions WithJson<T>(T obj);

    IRequestOptions WithFile(string name, string path);

    IRequestOptions WithTimeout(TimeSpan timeout);
}
