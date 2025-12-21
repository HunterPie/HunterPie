using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Settings.Types;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace HunterPie.Core.Converters;

public class SecretConverter : JsonConverter
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public override bool CanConvert(Type objectType) => objectType.GetInterfaces().Contains(typeof(Secret));

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        try
        {
            string value = reader.Value != null
            ? (string)Convert.ChangeType(reader.Value, typeof(string))
            : string.Empty;

            objectType.GetProperty(nameof(Secret.EncryptedValue))?
                .SetValue(existingValue, value);
        }
        catch (Exception err)
        {
            _logger.Error($"failed to deserialize secret: {err}");
        }

        return existingValue;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        object? prop = value?
                .GetType()
                .GetProperty(nameof(Secret.EncryptedValue))?
                .GetValue(value);

        serializer.Serialize(writer, prop);
    }
}