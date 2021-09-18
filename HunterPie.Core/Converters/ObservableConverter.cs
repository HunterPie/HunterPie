using HunterPie.Core.Architecture;
using Newtonsoft.Json;
using System;

namespace HunterPie.Core.Converters
{
    class ObservableConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Observable<object>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            
            var T = objectType.GetGenericArguments()[0];
            var value = reader.Value != null 
                ? Convert.ChangeType(reader.Value, T)
                : null;

            var observable = Activator.CreateInstance(objectType, value);
            objectType.GetProperty(nameof(Observable<object>.Value)).SetValue(existingValue, value);

            return observable;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var prop = value.GetType()
                        .GetProperty(nameof(Observable<object>.Value))
                        .GetValue(value);

            serializer.Serialize(writer, prop);
        }
    }
}
