using HunterPie.Core.Converters;
using Newtonsoft.Json;

namespace HunterPie.Core.Architecture
{
    [JsonConverter(typeof(ObservableConverter))]
    public class Observable<T> : Bindable
    {
        private T _value;

        [JsonProperty]
        public T Value
        {
            get => _value;
            set { SetValue(ref _value, value); }
        }

        [JsonConstructor]
        public Observable(T value)
        {
            Value = value;
        }

        public static implicit operator Observable<T>(T v)
        {
            return new Observable<T>(v);
        }

        public static implicit operator T(Observable<T> v)
        {
            return v.Value;
        }

        public static Observable<T> Default()
        {
            return new Observable<T>(default);
        }
    }
}
