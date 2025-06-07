﻿using HunterPie.Core.Settings.Adapters;

namespace HunterPie.UI.Settings.Converter.Model;

#nullable enable
public record PropertyData(
    string Name,
    string Description,
    string Group,
    object Value,
    bool RequiresRestart,
    PropertyCondition[] Conditions,
    ISettingAdapter? Adapter
);