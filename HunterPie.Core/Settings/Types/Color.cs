using HunterPie.Core.Architecture;

namespace HunterPie.Core.Settings.Types;

public class Color : Bindable
{
    public string Value { get; set => SetValue(ref field, value); }

    public Color(string value)
    {
        Value = value;
    }

    public static implicit operator Color(string v) => new(v);

    public static implicit operator string(Color v) => v.Value;
}