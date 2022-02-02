using HunterPie.Core.Settings.Types;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace HunterPie.Core.Converters
{
    public class FileSelectorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetInterfaces().Contains(typeof(IFileSelector));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            var value = reader.Value != null
                ? Convert.ChangeType(reader.Value, typeof(string))
                : null;

            objectType.GetProperty(nameof(IFileSelector.Current)).SetValue(existingValue, value);

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var prop = value.GetType()
                        .GetProperty(nameof(IFileSelector.Current))
                        .GetValue(value);

            serializer.Serialize(writer, prop);
        }
    }
}
