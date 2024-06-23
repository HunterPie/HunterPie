using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Converters;
using Newtonsoft.Json;

namespace HunterPie.Core.Settings.Types;

public class MonsterDetails
{
    [JsonConverter(typeof(ObservableHashSetConverter<MonsterConfiguration>))]
    public ObservableHashSet<MonsterConfiguration> Monsters { get; } = new();
}