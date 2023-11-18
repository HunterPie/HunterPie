using HunterPie.Core.Settings.Adapters;
using System;

namespace HunterPie.Core.Settings;

[AttributeUsage(AttributeTargets.Property)]
public class SettingAdapter : Attribute
{
    public ISettingAdapter Adapter { get; init; }

    public SettingAdapter(Type adapter)
    {
        Adapter = (ISettingAdapter)Activator.CreateInstance(adapter);
    }
}