using HunterPie.Core.Architecture;
using HunterPie.Core.Converters;
using Newtonsoft.Json;
using System;

namespace HunterPie.Core.Settings.Types;

[JsonConverter(typeof(RangeConverter))]
public class Range : Bindable
{
    public double Current { get; set => SetValue(ref field, Math.Max(Math.Min(value, Max), Min)); }
    public double Max { get; set => SetValue(ref field, value); }
    public double Min { get; set => SetValue(ref field, value); }
    public double Step { get; set => SetValue(ref field, value); }

    [JsonConstructor]
    public Range() { }

    public Range(double current, double max, double min, double step)
    {
        Max = max;
        Min = min;
        Step = step;
        Current = current;
    }
}