﻿using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;

namespace HunterPie.UI.Settings.Converter.Internal;

#nullable enable
internal class KeybindingConfigurationPropertyBuilder : IConfigurationPropertyBuilder
{
    public IConfigurationProperty Build(PropertyData data, GameProcessType game)
    {
        if (data.Value is not Keybinding value)
            throw new ArgumentException($"Property must be of type {nameof(Keybinding)}");

        return new KeybindingPropertyViewModel(value)
        {
            Name = data.Name,
            Description = data.Description,
            Group = data.Group,
            RequiresRestart = data.RequiresRestart,
            Condition = data.Condition,
        };
    }
}