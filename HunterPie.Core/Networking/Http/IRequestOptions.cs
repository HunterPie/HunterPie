namespace HunterPie.Core.Networking.Http;

public interface IRequestOptions
{
    void WithHeader(string key, string value);

    void WithQuery(string key, object? value);

    void WithJSON<T>(T obj) where T : class;

    void WithFile(string name, string path);
}
