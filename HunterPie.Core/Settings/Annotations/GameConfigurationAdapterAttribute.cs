using HunterPie.Core.Settings.Adapters;
using System;

namespace HunterPie.Core.Settings.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public class GameConfigurationAdapterAttribute(Type adapterType) : Attribute
{
    public ISettingAdapter Adapter { get; init; } = (ISettingAdapter)Activator.CreateInstance(adapterType);
}