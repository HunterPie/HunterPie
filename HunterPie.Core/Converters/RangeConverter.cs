using Range = HunterPie.Core.Settings.Types.Range;
using Newtonsoft.Json;
using System;

namespace HunterPie.Core.Converters
{
    class RangeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Range);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object value = Convert.ChangeType(reader.Value, typeof(double));

            objectType.GetProperty(nameof(Range.Current))
                .SetValue(existingValue, value);

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var prop = value.GetType()
                    .GetProperty(nameof(Range.Current))
                    .GetValue(value);

            serializer.Serialize(writer, prop);
        }
    }
}
