using Newtonsoft.Json;

namespace HunterPie.Core.Architecture
{
    public class Observable<T> : Notifiable
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

        public static Observable<T> Default()
        {
            return new Observable<T>(default);
        }
    }
}
