using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace HunterPie.Core.Settings.Types
{
    public interface IFileSelector
    {
        [JsonProperty]
        public string Current { get; set; }

        [JsonIgnore]
        public ObservableCollection<string> Elements { get; }
    }
}
