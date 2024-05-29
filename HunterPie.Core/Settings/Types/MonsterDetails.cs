using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Converters;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HunterPie.Core.Settings.Types;

public class MonsterDetails
{
    [JsonConverter(typeof(ObservableCollectionConverter<MonsterConfiguration>))]
    public ObservableCollection<MonsterConfiguration> Monsters { get; } = new ObservableCollection<MonsterConfiguration>();

    public MonsterConfiguration this[int index] => Monsters[index];
}