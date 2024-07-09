using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Converters;
using HunterPie.Core.Game.Data.Definitions;
using Newtonsoft.Json;
using System;

namespace HunterPie.Core.Settings.Types;

public class MonsterDetails
{
    [JsonConverter(typeof(ObservableHashSetConverter<PartGroupType>))]
    public ObservableHashSet<PartGroupType> AllowedPartGroups { get; } = new(Enum.GetValues<PartGroupType>());

    [JsonConverter(typeof(ObservableHashSetConverter<MonsterConfiguration>))]
    public ObservableHashSet<MonsterConfiguration> Monsters { get; } = new();
}