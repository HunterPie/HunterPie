using HunterPie.Core.Converters;
using Newtonsoft.Json;

namespace HunterPie.Core.Architecture;

[JsonConverter(typeof(ObservableConverter))]
public class Observable<T> : Bindable
{
    [JsonProperty]
    public T Value
    {
        get;
        set => SetValue(ref field, value);
    }

    [JsonConstructor]
    public Observable(T value)
    {
        Value = value;
    }

    public static implicit operator Observable<T>(T v) => new(v);

    public static implicit operator T(Observable<T> v) => v.Value;

    public static Observable<T> Default() => new(default);
}