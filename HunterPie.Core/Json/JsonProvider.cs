using Newtonsoft.Json;

#nullable enable
namespace HunterPie.Core.Json;

public static class JsonProvider
{

    private static readonly JsonSerializerSettings _deserializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        TypeNameHandling = TypeNameHandling.Auto
    };

    private static readonly JsonSerializerSettings _serializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
    };

    public static void Populate(string value, object target) =>
        JsonConvert.PopulateObject(value, target, _deserializerSettings);

    public static string Serialize(object? value) =>
        JsonConvert.SerializeObject(value, Formatting.Indented, _serializerSettings);

    public static T Deserialize<T>(string value) =>
        JsonConvert.DeserializeObject<T>(value)!;
}
#nullable restore
