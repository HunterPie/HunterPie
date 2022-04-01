using HunterPie.Core.Client.Configuration.Overlay;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HunterPie.Core.Settings.Types
{
    public class AbnormalityTrays
    {
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public ObservableCollection<AbnormalityWidgetConfig> Trays { get; set; } = new();

        public AbnormalityWidgetConfig this[int index] => Trays[index];

    }
}
