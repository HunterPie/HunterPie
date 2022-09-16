using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HunterPie.Core.Settings.Types;

public interface IFileSelector
{
    [JsonProperty]
    public string Current { get; set; }

    [JsonIgnore]
    public ObservableCollection<string> Elements { get; }
}
