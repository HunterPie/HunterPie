using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

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

    private static readonly Lazy<JsonSerializerSettings> _newSerializeSettings = new(() =>
    {
        var serializer = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore
        };
        serializer.Converters.Add(new StringEnumConverter());

        return serializer;
    });

    public static void Populate(string value, object target) =>
        JsonConvert.PopulateObject(value, target, _deserializerSettings);

    public static string Serializer(object? value, bool indented = false) =>
        JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, _newSerializeSettings.Value);

    public static T Deserializer<T>(string value) =>
        JsonConvert.DeserializeObject<T>(value, _newSerializeSettings.Value)!;

    [Obsolete("This method is deprecated, use Serializer instead.")]
    public static string Serialize(object? value) =>
        JsonConvert.SerializeObject(value, Formatting.Indented, _serializerSettings);

    [Obsolete("This method is deprecated, use Deserializer instead.")]
    public static T Deserialize<T>(string value) =>
        JsonConvert.DeserializeObject<T>(value)!;
}
#nullable restore
